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
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product createProductDto);
        Task<Product> UpdateProductAsync(Product updateProductDto);
        Task<bool> DeleteProductById(int id);
        Task<PagedResponseDto<Product>> GetProductsAsync(string search, int page, int limit);
    }
}
