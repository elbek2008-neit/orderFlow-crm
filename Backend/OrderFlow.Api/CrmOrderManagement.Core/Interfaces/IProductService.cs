using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeleteProductByIdAsync(int id);
        Task<PagedResponseDto<ProductDto>> GetProductsAsync(string search, int page, int limit);
    }
}
