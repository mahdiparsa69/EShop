using AutoMapper;
using EShop.Api.CustomFilters;
using EShop.Api.Models.RequstModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Service.Interfaces;
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
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, IRedisCacheService redisCacheService, ITokenService tokenService, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
            _tokenService = tokenService;
            _configuration = configuration;
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Order>> GetWithoutIncludeAsync([FromRoute] Guid id)
        {
            if (id == default)
                return BadRequest();

            var order = await _orderRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        [AuthorizationFilterWithFactory]
        //[ServiceFilter(typeof(AuthorizeByTokenFilterAsync))]
        public async Task<ActionResult<List<OrderCompactViewModel>>?> GetListAsync([FromQuery] string? name, [FromQuery] int offset = 0,
            [FromQuery] int count = 10)
        {
            var orders = await _orderRepository.GetListAsync(new OrderFilter()
            {
                Offset = offset,
                Count = count
            }, HttpContext.RequestAborted);

            if (!orders.Items.Any())
                return default;

            var result = _mapper.Map<List<Order>, List<OrderCompactViewModel>>(orders.Items);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] OrderCreateRequest orderCreateRequest)
        {
            /* if (orderCreateRequest == null)
                 return BadRequest();*/

            var order = _mapper.Map<Order>(orderCreateRequest);

            await _orderRepository.AddAsync(order, HttpContext.RequestAborted);

            var orderViewModel = _mapper.Map<Order, OrderCompactViewModel>(order);

            return Ok(orderViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> RemoveAsync([FromRoute] Guid id)
        {
            if (id == default)
                return BadRequest();

            var order = await _orderRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (order == null)
                return NotFound();

            await _orderRepository.Remove(id, HttpContext.RequestAborted);

            return Ok();
        }
    }
}
