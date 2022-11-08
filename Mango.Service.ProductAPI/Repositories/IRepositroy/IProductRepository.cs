using Mango.Service.ProductAPI.Models.Dtos;
using System.Collections.Generic;

namespace Mango.Service.ProductAPI.Repositories.IRepositroy
{
    public interface IProductRepository
    {
        //get products
        Task<IEnumerable<ProductDto>> GetProducts();
        // get product by Id
        Task<ProductDto> GetProductById(int productId);
        // createUpdate product
        Task<ProductDto> CreateUpdateProduct(ProductDto productDto);
        // delete product
        Task<bool> DeleteProduct(int productId);
    }
}
