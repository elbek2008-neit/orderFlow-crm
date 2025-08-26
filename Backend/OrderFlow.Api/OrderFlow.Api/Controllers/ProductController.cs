using CrmOrderManagement.Core.Dtos;
using AutoMapper;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Interfaces;
using CrmOrderManagement.Services.Service;
using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductController(IProductService productService, IMapper mapper)
        { _productService = productService;  _mapper = mapper; }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        { 
            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var updated = await _productService.UpdateProductAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteProductByIdAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetProducts(string search, int page, int limit)
        { 
            var products = await _productService.GetProductsAsync(search, page, limit);
            return Ok(products);
        }
    }       
}
