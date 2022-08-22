using AutoMapper;
using EShop.Api.Models.RequstModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWithoutIncludeAsync([FromRoute] Guid id)
        {
            if (id == default)
                return BadRequest();

            var order = _orderRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] string? name, [FromQuery] int offset = 0,
            [FromQuery] int count = 10)
        {
            var orderFilter = new OrderFilter()
            {
                Name = name,
                Offset = offset,
                Count = count
            };
            var orders = await _orderRepository.GetListAsync(orderFilter, HttpContext.RequestAborted);

            if (orders.Items == null)
                return default;

            var result = _mapper.Map<List<Order>, List<OrderCompactViewModel>>(orders.Items);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] OrderCreateRequest orderCreateRequest)
        {
            if (orderCreateRequest == null)
                return BadRequest();

            var order = _mapper.Map<Order>(orderCreateRequest);

            await _orderRepository.AddAsync(order, HttpContext.RequestAborted);

            var orderViewModel = _mapper.Map<Order, OrderCompactViewModel>(order);
            return Ok(orderViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            if (id == default)
                return BadRequest();

            var order = await _orderRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (order == null)
                return NotFound();

            _orderRepository.Remove(id, HttpContext.RequestAborted);

            return Ok();
        }
    }
}
