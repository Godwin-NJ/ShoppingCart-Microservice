using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;           
        }

        [Authorize]
        public async Task <IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartDeatilsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto resp = await _cartService.RemoveFromCartAync(cartDeatilsId);
            if (resp != null && resp.IsSuccess)
            {
                TempData["success"] = "cart updated succesfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartdto)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto resp = await _cartService.ApplyCouponAsync(cartdto);
            if (resp != null && resp.IsSuccess)
            {
                TempData["success"] = "coupon applied succesfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartdto)
        {
            cartdto.CartHeader.CouponCode = "";
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto resp = await _cartService.ApplyCouponAsync(cartdto);
            if (resp != null && resp.IsSuccess)
            {
                TempData["success"] = "coupon applied succesfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto resp = await _cartService.GetCartByUserIdAsync(userId);
            if (resp != null && resp.IsSuccess) 
            {
                CartDto cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(resp.Result));
                return cart;
            }
            return new CartDto();
        }

    }
}
