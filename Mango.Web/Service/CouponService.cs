using Mango.Web.Models;
using Mango.Web.Service.IService;

namespace Mango.Web.Service
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory _httpClient;

        public CouponService(IHttpClientFactory httpClient):base(httpClient)
        {
           _httpClient = httpClient;
        }

        public async Task<T> GetCouponAsync<T>(string couponCode, string token = null)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Data = couponCode,
                URL = SD.CouponAPIBase + "api/coupon/"+couponCode,
                AccessToken = token
            });
        }
    }
}
