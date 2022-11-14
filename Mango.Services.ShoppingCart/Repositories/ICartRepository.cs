using Mango.Services.ShoppingCart.Models.Dto;

namespace Mango.Services.ShoppingCart.Repositories
{
    public interface ICartRepository
    {
        //get cartdto
        Task<CartDto> GetCartByUserId(string userId);
        //create update cart
        Task<CartDto> CreateUpdateCart(CartDto cartDto);
        //remove product from cart
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ApplyCouponAsync(string userId, string couponCode); 
        Task<bool> RemoveCouponAsync(string userId); 
        //clear cart
        Task<bool> ClearCart(string userId);

    }
}
