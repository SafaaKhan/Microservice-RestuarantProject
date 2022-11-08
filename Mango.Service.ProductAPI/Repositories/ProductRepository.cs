using AutoMapper;
using Mango.Service.ProductAPI.Data;
using Mango.Service.ProductAPI.Models;
using Mango.Service.ProductAPI.Models.Dtos;
using Mango.Service.ProductAPI.Repositories.IRepositroy;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ProductAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            var product=_mapper.Map<Product>(productDto);
            if (product.ProductId > 0)
            {
                _db.Update(product);
            }
            else
            {
                _db.Add(product);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
                if(product == null)
                {
                    return false;
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x=>x.ProductId==productId);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
           var products = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(products);
        }
    }
}
