using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Interfaces;
using CrmOrderManagement.Infrastructure.Repositories.IRepositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id) 
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto) 
        {
            var product = _mapper.Map<Product>(createProductDto);
            _unitOfWork.Products.Add(product);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new Exception("product not found");

            _mapper.Map(updateProductDto, product);

            product.UpdateAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ProductDto>(product);
        }
        public async Task<bool> DeleteProductByIdAsync(int id) 
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) throw new Exception("Product not found");

            _unitOfWork.Products.Remove(product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public async Task<PagedResponseDto<ProductDto>> GetProductsAsync(string search, int page, int limit)
        {
            if (page <= 0) page = 1;
            if (limit <= 0) limit = 10;

            // Базовый запрос
            var query = _unitOfWork.Products.GetAll(); // IQueryable<Product>

            // Фильтрация по поиску
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            // Подсчет общего количества
            var total = await query.CountAsync();

            // Пагинация
            var products = await query
                .OrderBy(p => p.Name) // Сортировка по имени (можно кастомизировать)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Маппинг
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            // Рассчет общего количества страниц
            var totalPages = (int)Math.Ceiling(total / (double)limit);

            // Формируем ответ
            return new PagedResponseDto<ProductDto>
            {
                Data = productDtos,
                Meta = new MetaDataDto
                {
                    Total = total,
                    Page = page,
                    Limit = limit,
                    TotalPages = totalPages
                }
            };
        }
    }
}
