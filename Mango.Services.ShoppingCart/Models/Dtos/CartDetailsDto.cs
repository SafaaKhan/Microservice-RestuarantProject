using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCart.Models.Dto
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        //one cart header can have many cart details(one to many)
        public int CartHeaderId { get; set; }
        public CartHeaderDto CartHeader { get; set; }
        public int ProductId { get; set; }
        public  ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
