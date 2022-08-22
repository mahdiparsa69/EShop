using EShop.Api.Models;
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

        public BasketController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public async Task<IActionResult> AddBasket(BasketItem inputBasketItem)
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

                basket.BasketItems.Add(inputBasketItem);

                foreach (var i in redisBasket.BasketItems)
                {
                    i.TotalPrice += (long)((i.Price * i.Count * i.ItemDiscount) / 100);
                }

                await _redisCacheService.StoreAsync(userId.ToString(), basket, TimeSpan.FromHours(2.0));
            }
            else
            {
                foreach (var item in redisBasket.BasketItems)
                {
                    if (item.ProductId == inputBasketItem.ProductId)
                    {
                        item.Count += inputBasketItem.Count;
                    }
                }

                redisBasket.BasketItems.Add(inputBasketItem);

                foreach (var i in redisBasket.BasketItems)
                {
                    i.TotalPrice += (long)((i.Price * i.Count * i.ItemDiscount) / 100);
                }
                await _redisCacheService.StoreAsync(userId.ToString(), redisBasket, TimeSpan.FromHours(2.0));

            }
            return Ok();
        }

        [HttpGet("{userId:guid'}")]
        public async Task<IActionResult> FinalizeToOrder(Guid userId)
        {
            if (userId == default)
                return BadRequest();

            var redisBasket = await _redisCacheService.FetchAsync<Basket>(userId.ToString());

            if (redisBasket == null)
                return NotFound("سبد خرید خالی می باشد");

            var order = new Order()
            {
                Id = Guid.NewGuid(),

                UserId = redisBasket.UserId,
            };

            foreach (var basketItem in redisBasket.BasketItems)
            {
                var orderItem = new OrderItem()
                {
                    ProductId = basketItem.ProductId,

                    OrderId = order.Id,

                    ProductPrice = basketItem.Price,

                    Count = basketItem.Count,
                };

                order.OrderItems.Add(orderItem);
            }

        }
    }
}
