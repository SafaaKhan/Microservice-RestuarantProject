using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCart.Models.Dto
{
    public class CartDetailsDto
    {
        [Key]
        public int CartDetailsId { get; set; }
        //one cart header can have many cart details(one to many)
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeaderDto CartHeader { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public  ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
