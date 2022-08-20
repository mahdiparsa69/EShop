using AutoMapper;
using EShop.Api.Models.ViewModels;
using EShop.Domain.Filters;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Repository.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository repo, IMapper mapper)
        {
            _productRepository = repo;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return Ok("good");
        }

        [HttpPost]
        [Route("AddAsync")]
        public async Task<IActionResult> AddAsync(Product product, CancellationToken cancellationToken)
        {
            _productRepository.AddAsync(product, cancellationToken);
            return Ok($"product {product.Name}:{product.Id} is added");
        }

        [HttpGet]
        [Route("GetWithoutIncludeAsync")]
        public async Task<IActionResult> GetWithoutIncludeAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetWithoutIncludeAsync(id, cancellationToken);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetListAsync")]
        public async Task<IActionResult> GetListAsync(CancellationToken cancellationToken)
        {
            ProductFilter productFilter = new ProductFilter();

            productFilter.Count = 10;
            var result = await _productRepository.GetListAsync(productFilter, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
        {
            await _productRepository.Remove(id, cancellationToken);
            return Ok("Deleted Successfully");
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateRequest productUpdateRequest, CancellationToken cancellationToken)
        {
            if (productUpdateRequest.Id == default)
            {
                return NotFound();
            }

            var result = await _productRepository.GetWithoutIncludeAsync(productUpdateRequest.Id, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            _mapper.Map<ProductUpdateRequest, Product>(productUpdateRequest, result);

            await _productRepository.Update(result, cancellationToken);

            return Ok($"Product {result.Name}:{result.Id} Updated Successfully");
        }
    }
}
