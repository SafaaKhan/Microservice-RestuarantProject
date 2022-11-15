using Mango.Services.CouponAPI.Models.Dtos;
using Mango.Services.CouponAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponAPIController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _responseDto;
        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            this._responseDto=new ResponseDto();
        }

       // [Authorize] does not work
        [HttpGet("{code}")]
        public async Task<object> GetDiscoutForCode(string code)
        {
            try
            {
                var coupon= await _couponRepository.GetCouponByCodeAsync(code);
                _responseDto.Result = coupon;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = new List<string> { ex.ToString() };
            }
            return _responseDto;
        }
    }
}
