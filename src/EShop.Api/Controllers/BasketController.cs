using EShop.Api.Models;
using EShop.Domain.Enums;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BasketController(IRedisCacheService redisCacheService, IOrderRepository orderRepository, ITransactionRepository transactionRepository, IOrderItemRepository orderItemRepository)
        {
            _redisCacheService = redisCacheService;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _orderItemRepository = orderItemRepository;
        }

        [HttpPost("AddBasket")]
        public async Task<IActionResult> AddBasket([FromBody] BasketItem inputBasketItem)
        {
            Guid userId = Guid.Parse("F9168C5E-CEB2-4faa-B6BF-329BF39FA1E4");

            var redisBasket = await _redisCacheService.FetchAsync<Basket>(userId.ToString());

            if (redisBasket == null)
            {
                Basket basket = new Basket()
                {
                    Id = Guid.NewGuid(),

                    UserId = userId,

                    BasketItems = new List<BasketItem>(),
                };
                inputBasketItem.BasketId = basket.Id;

                if (inputBasketItem.ItemDiscount > 0)
                    inputBasketItem.TotalPrice = (long)((inputBasketItem.Price * inputBasketItem.Count) - ((inputBasketItem.ItemDiscount * inputBasketItem.Price * inputBasketItem.Count) / 100));

                inputBasketItem.TotalPrice = (long)(inputBasketItem.Price * inputBasketItem.Count);

                basket.BasketItems.Add(inputBasketItem);

                basket.TotalAmount = inputBasketItem.TotalPrice;

                await _redisCacheService.StoreAsync(userId.ToString(), basket, TimeSpan.FromHours(2.0));
            }
            else
            {
                var r = redisBasket.BasketItems.Where(x => x.ProductId == inputBasketItem.ProductId).FirstOrDefault();

                if (r != null)
                {
                    r.Count += inputBasketItem.Count;
                }
                else
                {
                    inputBasketItem.BasketId = redisBasket.Id;

                    redisBasket.BasketItems.Add(inputBasketItem);
                }

                foreach (var i in redisBasket.BasketItems)
                {
                    if (i.ItemDiscount > 0)
                        i.TotalPrice = (long)((i.Price * i.Count) - ((i.ItemDiscount * i.Price * i.Count) / 100));

                    i.TotalPrice = (long)(i.Price * i.Count);
                }

                foreach (var item in redisBasket.BasketItems)
                {
                    redisBasket.TotalAmount += item.TotalPrice;
                }

                await _redisCacheService.StoreAsync(userId.ToString(), redisBasket, TimeSpan.FromHours(2.0));
            }

            return Ok();
        }

        [HttpGet("finalizetoorder/{userId:guid}")]
        public async Task<IActionResult> FinalizeToOrder([FromRoute] Guid userId)
        {
            if (userId == default)
                return BadRequest();

            var redisBasket = await _redisCacheService.FetchAsync<Basket>(userId.ToString());

            if (redisBasket?.BasketItems == null)
                return NotFound("سبد خرید خالی می باشد");

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = redisBasket.UserId,
                TotalAmount = redisBasket.TotalAmount,
                OrderItems = redisBasket.BasketItems.Select(x => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = x.ProductId,
                    ProductPrice = x.Price,
                    Count = x.Count,
                }).ToList()
            };

            await _orderRepository.AddAsync(order, HttpContext.RequestAborted);

            return Ok();
        }

        [HttpGet("payment/{orderId:guid}")]
        public async Task<IActionResult> Payment([FromRoute] Guid orderId)
        {
            var order = await _orderRepository.GetWithoutIncludeAsync(orderId, HttpContext.RequestAborted);

            if (order.IsPaid == false)
            {
                Transaction transaction = new Transaction()
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    Amount = order.FinalAmount,
                    Status = (TransactionStatus)new Random().Next(1, 2),
                    UserId = order.UserId,
                };

                if (transaction.Status == TransactionStatus.Successful)
                {
                    order.IsPaid = true;
                    transaction.ErrorMessage = " Transaction Succeded";
                    transaction.PaymentCode = Guid.NewGuid().ToString();
                    //order.Transactions.Add(transaction);
                }
                if (transaction.Status == TransactionStatus.UnSuccessful)
                {
                    order.IsPaid = false;
                    transaction.ErrorMessage = "Error in Doing Transaction";
                }

                await _transactionRepository.AddAsync(transaction, HttpContext.RequestAborted);

                await _orderRepository.Update(order, HttpContext.RequestAborted);

                return Ok();
            }

            return BadRequest("این سفارش قبلا پرداخت شده است.");

        }
    }
}
