using Mango.Web.Models.Dtos;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
       Task<T> GetCouponAsync<T>(string couponCode, string token = null);

    }
}
