using AutoMapper;
using EShop.Api.Models.RequestModels;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Service.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        private readonly IRedisCacheService _redisCacheService;

        private readonly IMapper _mapper;
        public ProductController(IProductRepository productRepository, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _productRepository = productRepository;

            _mapper = mapper;

            _redisCacheService = redisCacheService;
        }

        [HttpGet("GetFromRedis")]
        public async Task<IActionResult> GetFromRedis()
        {
            var cachedData = await _redisCacheService.FetchAsync("product", () =>
                _productRepository.GetListAsync(new ProductFilter()
                {
                    Offset = 0,
                    Count = int.MaxValue
                }, HttpContext.RequestAborted), TimeSpan.FromHours(2.0));

            if (cachedData != null)
            {
                var cachedProduct = _mapper.Map<List<Product>, List<ProductViewModel>>(cachedData.Items);
                return Ok(cachedProduct);
            }

            return NotFound("Item Not Fount");
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] ProductCreateRequest productCreateRequest)
        {
            var product = _mapper.Map<Product>(productCreateRequest);

            await _productRepository.AddAsync(product, HttpContext.RequestAborted);

            BackgroundJob.Enqueue(() => Console.WriteLine($"Product {product.Name} with ID {product.Id} Inserted to database"));

            var productViewModel = _mapper.Map<Product, ProductViewModel>(product);

            return Ok(productViewModel);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetWithoutIncludeAsync([FromRoute] Guid id)
        {
            if (id == default)
                return BadRequest();

            var product = await _productRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (product == null)
                return NotFound();

            var productViewModel = _mapper.Map<Product, ProductViewModel>(product);

            return Ok(productViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] string? name, [FromQuery] int offSet = 0, [FromQuery] int count = 10)
        {
            var products = await _productRepository.GetListAsync(new ProductFilter()
            {
                Name = name,
                Offset = offSet,
                Count = count
            }, HttpContext.RequestAborted);

            if (products.Items == null)
                return default;

            var result = _mapper.Map<List<Product>, List<ProductViewModel>>(products.Items);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remove([FromRoute] Guid id)
        {
            if (id == default)
                return BadRequest();

            var product = await _productRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (product == null)
                return NotFound();

            await _productRepository.Remove(product, HttpContext.RequestAborted);

            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] ProductUpdateRequest productUpdateRequest)
        {
            if (id == default)
                return BadRequest();

            var product = await _productRepository.GetWithoutIncludeAsync(id, HttpContext.RequestAborted);

            if (product == null)
                return NotFound();

            _mapper.Map<ProductUpdateRequest, Product>(productUpdateRequest, product);

            await _productRepository.Update(product, HttpContext.RequestAborted);

            var productViewModel = _mapper.Map<Product, ProductViewModel>(product);

            return Ok(productViewModel);
        }


    }
}
