using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCart.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        //one cart header can have many cart details(one to many)
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public  Product Product { get; set; }
        public int Count { get; set; }
    }
}
