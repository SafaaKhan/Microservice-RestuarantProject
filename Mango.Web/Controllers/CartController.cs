using Mango.Web.Models.Dtos;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, 
            ICartService cartService,
            ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            //userId
            var userId = User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
            //accesstoken
            var acccessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCouponAsync<ResponseDto>(cartDto, acccessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));

            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            //userId
            var userId = User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
            //accesstoken
            var acccessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCouponAsync<ResponseDto>(cartDto.CartHeader.UserId, acccessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));

            }
            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            //userId
            var userId = User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
            //accesstoken
            var acccessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, acccessToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));

            }
            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var acccessToken = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.CheckoutAsync<ResponseDto>(cartDto.CartHeader, acccessToken);
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception ex)
            {
                return View(cartDto);
            }
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            //userId
            var userId = User.Claims.Where(x => x.Type == "sub")?.FirstOrDefault()?.Value;
            //accesstoken
            var acccessToken=await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, acccessToken);

            CartDto cartDto = new();

            if (response != null && response.IsSuccess)
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));

            }

            if (cartDto.CartHeader != null)
            {
                //get coupon info
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var resp = await _couponService.GetCouponAsync<ResponseDto>(cartDto.CartHeader.CouponCode, acccessToken);
                    if (resp != null && resp.IsSuccess)
                    {
                        var couponObj = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
                        cartDto.CartHeader.DiscoutTotal = couponObj.DiscountAmount;
                    }
                }
                foreach(var details in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (details.Product.Price * details.Count);
                }
                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscoutTotal;
            }

            return cartDto;
        }
    }
}
