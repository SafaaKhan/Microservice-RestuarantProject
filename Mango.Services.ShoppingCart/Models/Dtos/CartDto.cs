namespace Mango.Services.ShoppingCart.Models.Dto
{
    public class CartDto //viewModel of the shoppting cart
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
