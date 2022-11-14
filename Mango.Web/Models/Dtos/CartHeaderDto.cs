namespace Mango.Web.Models.Dtos
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string CouponCode { get; set; } = string.Empty;
        public double OrderTotal { get; set; }
    }
}
