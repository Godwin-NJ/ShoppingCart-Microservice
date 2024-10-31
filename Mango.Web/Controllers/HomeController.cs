using IdentityModel;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController( IProductService productService, ICartService cartService)
        {          
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> productList = new();
            ResponseDto? resp = await _productService.GetProducts();

            if (resp != null && resp.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(resp.Result));
            }
            else
            {
                TempData["error"] = resp?.Message;
            }

            return View(productList);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto model = new();
            ResponseDto? resp = await _productService.GetProductById(productId);

            if (resp != null && resp.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));
            }
            else
            {
                TempData["error"] = resp?.Message;
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            CartDto cartdto = new()
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(x => x.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                }

            };

            CartDetailsDto cartdetail = new()
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId,                
            };

            List<CartDetailsDto> cartDetailsDto = new() { cartdetail };

            cartdto.CartDetails = cartDetailsDto;

            ProductDto model = new();
            ResponseDto? resp = await _cartService.UpsertCartAsync(cartdto);

            if (resp != null && resp.IsSuccess)
            {
                TempData["success"] = "item has been added to the shopping cart";
               return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = resp?.Message;
            }

            return View(productDto);
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
    }
}
