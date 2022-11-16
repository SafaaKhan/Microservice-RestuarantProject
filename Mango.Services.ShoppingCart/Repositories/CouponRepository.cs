using Mango.Services.ShoppingCart.Models.Dto;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCart.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly HttpClient _httpClient;
        public CouponRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response= await _httpClient.GetAsync($"api/coupon/{couponName}");
            var apiContect = await response.Content.ReadAsStringAsync();
            var resp =  JsonConvert.DeserializeObject<ResponseDto>(Convert.ToString(apiContect));
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
