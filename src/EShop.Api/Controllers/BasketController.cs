using AutoMapper;
using EShop.Api.Models;
using EShop.Api.Models.RequstModels;
using EShop.Api.Models.ViewModels;
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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public BasketController(IRedisCacheService redisCacheService,
            IOrderRepository orderRepository,
            ITransactionRepository transactionRepository,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository, IMapper mapper)
        {
            _redisCacheService = redisCacheService;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> AddBasketAsync([FromBody] BasketItemRequest request)
        {
            Guid userId = Guid.Parse("F9168C5E-CEB2-4faa-B6BF-329BF39F9408");

            var basketCacheKey = $"basket-{userId}";

            var cachedBasket = await _redisCacheService.FetchAsync<Basket>(basketCacheKey);

            if (request.ProductId == default)
                return BadRequest("Invalid product ID");

            var product = await _productRepository.GetWithoutIncludeAsync(request.ProductId, HttpContext.RequestAborted);

            if (product == null)
                return NotFound("Product not found");

            if (cachedBasket == null)
            {
                var basket = new Basket
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    BasketItems = new List<BasketItem>()
                };

                var basketItem = new BasketItem
                {
                    Count = request.Count,
                    Product = _mapper.Map<Product, ProductViewModel>(product),
                    Amount = product.Price * request.Count,
                    DiscountAmount = product.DiscountPercent > 0
                        ? (long)(product.Price * product.DiscountPercent * request.Count) / 100
                        : 0,
                };

                basketItem.FinalAmount = basketItem.Amount - basketItem.DiscountAmount;

                basket.TotalAmount = basketItem.FinalAmount;

                basket.BasketItems.Add(basketItem);
                basket.DiscountAmount = basketItem.DiscountAmount;

                await _redisCacheService.StoreAsync(basketCacheKey, basket, TimeSpan.FromHours(2));
            }
            else
            {
                cachedBasket.BasketItems ??= new List<BasketItem>();

                var cachedBasketItem = cachedBasket.BasketItems.FirstOrDefault(x => x.Product.Id == product.Id);

                if (cachedBasketItem != null)
                {
                    cachedBasketItem.Count += request.Count;
                    cachedBasketItem.Amount += product.Price * request.Count;
                    cachedBasketItem.DiscountAmount += product.DiscountPercent > 0
                        ? (long)(product.Price * product.DiscountPercent * request.Count) / 100
                        : 0;
                    cachedBasketItem.FinalAmount = cachedBasketItem.Amount - cachedBasketItem.DiscountAmount;
                }
                else
                {
                    var basketItem = new BasketItem
                    {
                        Count = request.Count,
                        Product = _mapper.Map<Product, ProductViewModel>(product),
                        Amount = product.Price * request.Count,
                        DiscountAmount = product.DiscountPercent > 0
                            ? (long)(product.Price * product.DiscountPercent * request.Count) / 100
                            : 0,
                    };
                    basketItem.FinalAmount = basketItem.Amount - basketItem.DiscountAmount;
                    cachedBasket.BasketItems.Add(basketItem);
                    //cachedBasket.TotalAmount = cachedBasket.BasketItems.Sum(x => x.FinalAmount);
                }

                cachedBasket.TotalAmount = cachedBasket.BasketItems.Sum(x => x.FinalAmount);
                cachedBasket.DiscountAmount = cachedBasket.BasketItems.Sum(x => x.DiscountAmount);

                await _redisCacheService.StoreAsync(basketCacheKey, cachedBasket, TimeSpan.FromHours(2));
            }

            return Ok(cachedBasket);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> RemoveFromBasketItem([FromRoute] Guid userId, [FromBody] BasketItemRequest request)
        {
            if (userId == default)
                return BadRequest("Basket ID is not valid");

            if (request.Count < 1)
                return BadRequest("Count is not Valid");

            var basketCachKey = $"basket-{userId}";

            var cachedBasket = await _redisCacheService.FetchAsync<Basket>(basketCachKey);

            if (cachedBasket == null)
                return NotFound("Basket Not Found");

            var cachedBasketItem = cachedBasket.BasketItems.FirstOrDefault(x => x.Product.Id == request.ProductId);

            if (cachedBasketItem == null)
                return NotFound("Basket Item Not Found");

            if (cachedBasketItem.Count < request.Count)
                return BadRequest("Request Count is Wrong");

            cachedBasketItem.Count -= request.Count;

            if (cachedBasketItem.Count == 0)
            {
                cachedBasket.BasketItems.Remove(cachedBasketItem);
            }

            cachedBasketItem.Amount -= cachedBasketItem.Product.Price * request.Count;

            cachedBasketItem.DiscountAmount -= cachedBasketItem.Product.DiscountPercent > 0
                ? (long)(cachedBasketItem.Product.Price * cachedBasketItem.Product.DiscountPercent * request.Count) / 100
                : 0;

            cachedBasketItem.FinalAmount = cachedBasketItem.Amount - cachedBasketItem.DiscountAmount;

            cachedBasket.TotalAmount = cachedBasket.TotalAmount = cachedBasket.BasketItems.Sum(x => x.FinalAmount);

            cachedBasket.DiscountAmount = cachedBasket.BasketItems.Sum(x => x.DiscountAmount);

            await _redisCacheService.StoreAsync(basketCachKey, cachedBasket, TimeSpan.FromHours(2));

            return Ok();
        }



        [HttpPost("finalizetoorder/{userId:guid}")]
        public async Task<IActionResult> FinalizeToOrderAsync([FromRoute] Guid userId)
        {
            if (userId == default)
                return BadRequest("User ID is not valid");

            var basketCacheKey = $"basket-{userId}";

            var cachedBasket = await _redisCacheService.FetchAsync<Basket>(basketCacheKey);

            if (cachedBasket?.BasketItems == null)
                return NotFound("Basket Item is empty");

            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = cachedBasket.UserId,
                DiscountAmount = cachedBasket.DiscountAmount,
                TotalAmount = cachedBasket.TotalAmount + cachedBasket.DiscountAmount,
                FinalAmount = cachedBasket.TotalAmount,

                OrderItems = cachedBasket.BasketItems.Select(x => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = x.Product.Id,
                    ProductPrice = x.Product.Price,
                    Count = x.Count,
                    ProductDiscountPercent = x.Product.DiscountPercent,
                    TotalPrice = x.FinalAmount
                }).ToList(),
            };

            //Add Order to repository And Add OrederItems to orderItem table automatically becase of relation between order and orderItems
            await _orderRepository.AddAsync(order, HttpContext.RequestAborted);

            return Ok();
        }

        [HttpPost("payment/{orderId:guid}")]
        public async Task<IActionResult> Payment([FromRoute] Guid orderId)
        {
            var order = await _orderRepository.GetWithoutIncludeAsync(orderId, HttpContext.RequestAborted);

            if (order.IsPaid == false)
            {
                var random = new Random().Next(1, 5);
                Transaction transaction = new Transaction()
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    Amount = order.FinalAmount,
                    Status = (TransactionStatus)(random != 1 ? 1 : 2),
                    UserId = order.UserId,
                };

                if (transaction.Status == TransactionStatus.Successful)
                {
                    order.IsPaid = true;
                    transaction.ErrorMessage = "Transaction Succeded";
                    transaction.PaymentCode = Guid.NewGuid().ToString();
                    //order.Transactions.Add(transaction);
                }
                if (transaction.Status == TransactionStatus.UnSuccessful)
                {
                    order.IsPaid = false;
                    transaction.ErrorMessage = "Transaction Encountered An Error";
                }

                await _transactionRepository.AddAsync(transaction, HttpContext.RequestAborted);

                await _orderRepository.Update(order, HttpContext.RequestAborted);

                return Ok();
            }

            return BadRequest("Order Can Not Payed Two Times.It is payed before ");

        }
    }
}
