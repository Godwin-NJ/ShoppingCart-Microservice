using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService) 
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> CreateCouponAsync(CouponDto coupondto)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = coupondto,
                Url = StaticDetails.CouponApiBaseUrl + $"/api/Coupon/CreateCoupon"
            });
            return resp;
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.CouponApiBaseUrl + $"/api/Coupon/DeleteCoupon/{id}"
            });
            return resp;
        }

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
          var resp =  await  _baseService.SendAsync(new RequestDto()
            {
              ApiType = StaticDetails.ApiType.GET,
              Url = StaticDetails.CouponApiBaseUrl + "/api/Coupon"
            });
            return resp;
        }

        public async Task<ResponseDto> GetCouponAsync(string couponCode)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponApiBaseUrl + $"/api/Coupon/GetByCode/{couponCode}"
            });
            return resp;
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int couponId)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponApiBaseUrl + $"/api/Coupon/{couponId}"
            });
            return resp;
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto coupondto)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = coupondto,
                Url = StaticDetails.CouponApiBaseUrl + $"/api/coupon/UpdateCoupon"
            });
            return resp;
        }
    }
}
