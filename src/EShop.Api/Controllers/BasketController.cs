using AutoMapper;
using EasyNetQ;
using EShop.Api.Models;
using EShop.Api.Models.RequstModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Common.BrokerMessages;
using EShop.Domain.Enums;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IBus _bus;
        private readonly IAsyncJobProducer _asyncJobProducer;
        private readonly IMapper _mapper;

        public BasketController(IRedisCacheService redisCacheService,
            IOrderRepository orderRepository,
            ITransactionRepository transactionRepository,
            IOrderItemRepository orderItemRepository,
            IProductRepository productRepository, IMapper mapper, IBus bus, IAsyncJobProducer asyncJobProducer)
        {
            _redisCacheService = redisCacheService;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _bus = bus;
            _asyncJobProducer = asyncJobProducer;
        }

        [HttpPost]
        public async Task<ActionResult<Basket>> AddBasketAsync([FromBody] BasketItemRequest request)
        {
            if (request.ProductId == default || request.ProductId == null)
                return BadRequest("Invalid product ID");

            Guid userId = Guid.Parse("F9168C5E-CEB2-4faa-B6BF-329BF39F9408");

            var basketCacheKey = $"basket-{userId}";

            var cachedBasket = await _redisCacheService.FetchAsync<Basket>(basketCacheKey);

            var product = await _productRepository.GetWithoutIncludeAsync(request.ProductId.Value, HttpContext.RequestAborted);

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

                return Ok(basket);
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
                }

                cachedBasket.TotalAmount = cachedBasket.BasketItems.Sum(x => x.FinalAmount);

                cachedBasket.DiscountAmount = cachedBasket.BasketItems.Sum(x => x.DiscountAmount);

                await _redisCacheService.StoreAsync(basketCacheKey, cachedBasket, TimeSpan.FromHours(2));
            }

            return Ok(cachedBasket);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult<Basket>> RemoveFromBasketItem([FromRoute] Guid userId, [FromBody] BasketItemRequest request)
        {
            if (userId == default)
                return BadRequest("Basket ID is not valid");

            if (request.Count < 1)
                return BadRequest("Count is not Valid");

            var basketCacheKey = $"basket-{userId}";

            var cachedBasket = await _redisCacheService.FetchAsync<Basket>(basketCacheKey);

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

            await _redisCacheService.StoreAsync(basketCacheKey, cachedBasket, TimeSpan.FromHours(2));

            return Ok(cachedBasket);
        }



        [HttpPost("finalize/{userId:guid}")]
        public async Task<ActionResult<Order>> FinalizeToOrderAsync([FromRoute] Guid userId)
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

            return Ok(order);
        }

        [HttpPost("payment/{orderId:guid}")]
        public async Task<ActionResult<Transaction>> Payment([FromRoute] Guid orderId)
        {
            var order = await _orderRepository.GetWithoutIncludeAsync(orderId, HttpContext.RequestAborted);

            if (order == null)
                return BadRequest();

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


                TransactionMessage tx = new TransactionMessage();

                tx.TextMessage = $"New Transaction Added. Id is : {transaction.Id}";

                await _asyncJobProducer.PublishAsync(tx, HttpContext.RequestAborted);

                Console.WriteLine("*********************Message Published!!!**********************************");

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

                return Ok(transaction);
            }

            return BadRequest("Order Can Not Payed Two Times.It is payed before ");

        }

        [HttpGet("transactions")]
        public async Task<ActionResult<List<TransactionViewModel>>> GetListTransactionAsync([FromQuery] int offset = 0, [FromQuery] int count = 10)
        {
            TransactionFilter filter = new TransactionFilter()
            {
                Offset = offset,
                Count = count
            };

            var transactions = await _transactionRepository.GetListAsync(filter, HttpContext.RequestAborted);

            if (transactions.Items.Count == 0)
                return default;

            Response.Headers.Add("nextUrl", $"?offset={offset + count}&count={count}");

            var result = _mapper.Map<List<Transaction>, List<TransactionViewModel>>(transactions.Items);

            return Ok(result);
        }

    }
}
