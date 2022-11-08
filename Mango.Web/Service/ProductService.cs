using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IHttpClientFactory _httpClient;

        public ProductService(IHttpClientFactory httpClient):base(httpClient)
        {
           _httpClient = httpClient;
        }

        public async Task<T> CreateProductAsync<T>(ProductDto productDto)
        {
           return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data =productDto,
                URL=SD.ProductAPIBase+ "api/products",
                AccessToken=""
            });
        }

        public async Task<T> DeleteProductAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                URL = SD.ProductAPIBase + "api/products"+id,
                AccessToken = ""
            });
        }

        public async Task<T> GetAllProductsAsync<T>()
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                URL = SD.ProductAPIBase + "api/products",
                AccessToken = ""
            });
        }

        public async Task<T> GetProductByIdAsync<T>(int id)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                URL = SD.ProductAPIBase + "api/products" + id,
                AccessToken = ""
            });
        }

        public async Task<T> UpdateProductAsync<T>(ProductDto productDto)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                URL = SD.ProductAPIBase + "api/products",
                AccessToken = ""
            });
        }
    }
}
