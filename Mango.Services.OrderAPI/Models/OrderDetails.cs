using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        //one cart header can have many cart details(one to many)
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeader OrderHeader { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public string ProdcutName { get; set; }
        public double Price { get; set; }

    }
}
