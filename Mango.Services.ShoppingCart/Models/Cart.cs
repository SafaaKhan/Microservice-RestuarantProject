namespace Mango.Services.ShoppingCart.Models
{
    public class Cart //viewModel of the shoppting cart
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
