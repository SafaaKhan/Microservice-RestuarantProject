﻿using Mango.Web.Models.Dtos;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
           _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? productDtosList = new List<ProductDto>();
            var response = await _productService.GetAllProductsAsync<ResponseDto>();
            if(response!=null && response.IsSuccess)
            {
                productDtosList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result)?? string.Empty);
            }
            return View(productDtosList);
        }
    }
}
