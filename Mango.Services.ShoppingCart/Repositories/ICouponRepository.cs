using Mango.Services.ShoppingCart.Models.Dto;

namespace Mango.Services.ShoppingCart.Repositories
{
    public interface ICouponRepository
    {
       Task<CouponDto> GetCoupon(string couponName);
    }
}
