using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCart.Models.Dto
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; }=string.Empty;
        public string CouponCode { get; set; } = string.Empty;
    }
}
