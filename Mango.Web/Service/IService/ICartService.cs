﻿using Mango.Web.Models.Dtos;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
       Task<T> GetCartByUserIdAsync<T>(string userId, string? token=null);
       Task<T> AddToCartAsync<T>(CartDto cartDto, string? token=null);
       Task<T> UpdateCartAsync<T>(CartDto cartDto, string? token=null);
       Task<T> RemoveFromCartAsync<T>(int cartId, string? token=null); //cart details id
       Task<T> ApplyCouponAsync<T>(CartDto cartDto, string token = null);
       Task<T> RemoveCouponAsync<T>(string userId, string token = null);
       Task<T> CheckoutAsync<T>(CartHeaderDto cartHeaderDto, string token = null);

    }
}
