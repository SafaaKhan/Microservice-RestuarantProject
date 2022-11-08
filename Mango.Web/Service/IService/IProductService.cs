using Mango.Web.Models.Dtos;

namespace Mango.Web.Service.IService
{
    public interface IProductService:IBaseService
    {
        Task<T> GetAllProductsAsync<T>();
        Task<T> GetProductByIdAsync<T>(int id);
        Task<T> UpdateProductAsync<T>(ProductDto productDto);
        Task<T> CreateProductAsync<T>(ProductDto productDto);
        Task<T> DeleteProductAsync<T>(int id);
    }
}
