using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService  _couponService;
        public CouponController(ICouponService couponService) 
        {             
            _couponService = couponService;
        }
        /// <summary>
        /// Coupon Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> couponList = new();
            ResponseDto? resp = await _couponService.GetAllCouponsAsync();

            if (resp != null && resp.IsSuccess)
            {
                couponList = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(resp.Result));
            }
            else
            {
                TempData["error"] = resp?.Message;
            }

            return View(couponList);
        }

        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto dto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? resp = await _couponService.CreateCouponAsync(dto);
                
                if (resp != null && resp.IsSuccess)
                {
                  return  RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = resp?.Message;
                }

            }
            return View(dto);
        }

        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
			ResponseDto? resp = await _couponService.GetCouponByIdAsync(couponId);

			if (resp != null && resp.IsSuccess)
			{				
				CouponDto? data = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));

              return View(data);
			}
            else
            {
                TempData["error"] = resp?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCoupon(CouponDto couponDto)
        {
			ResponseDto? resp = await _couponService.DeleteCouponAsync(couponDto.CouponId);

			if (resp != null && resp.IsSuccess)
			{
				TempData["success"] = "coupon deleted successful";
				return RedirectToAction(nameof(CouponIndex)); ;
			}
            else
            {
                TempData["error"] = resp?.Message;
            }
            return View(couponDto); ;
        }
    }
}
