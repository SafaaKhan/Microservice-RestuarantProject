using Mango.Web.Models;
using Mango.Web.Models.Dtos;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(ILogger<HomeController> logger,
            IProductService productService,
            ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> productDtos = new List<ProductDto>();
            ResponseDto response= await _productService.GetAllProductsAsync<ResponseDto>("");
            if(response!=null && response.IsSuccess)
            {
                productDtos = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(productDtos);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductDto productDto = new ProductDto();
            ResponseDto response = await _productService.GetProductByIdAsync<ResponseDto>(productId,"");
            if (response != null && response.IsSuccess)
            {
                productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            return View(productDto);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDto productDtoP)
        {
            CartDto cartDto = new CartDto()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new CartDetailsDto()
            {
                Count=productDtoP.Count,
                ProductId=productDtoP.ProductId
            };

            //populate product 
            var res = await _productService.GetProductByIdAsync<ResponseDto>(productDtoP.ProductId, "");
            if(res!=null && res.IsSuccess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(res.Result));
            }

            List<CartDetailsDto> cartDetailsList = new List<CartDetailsDto>();
            cartDetailsList.Add(cartDetails);
            cartDto.CartDetails = cartDetailsList;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp=await _cartService.AddToCartAsync<ResponseDto>(cartDto,accessToken);
            if(addToCartResp!=null && addToCartResp.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(productDtoP);
           }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies","oidc");
        }
    }
}