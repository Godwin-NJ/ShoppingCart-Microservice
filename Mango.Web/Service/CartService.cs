using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService baseService) 
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> ApplyCouponAsync(CartDto dto)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url = StaticDetails.ShoppingCartApiBaseUrl + $"/api/cart/applycoupon"
            });
            return resp;
        }
          

        public async Task<ResponseDto> GetCartByUserIdAsync(string UserId)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ShoppingCartApiBaseUrl + $"/api/cart/GetCart/{UserId}"
            });
            return resp;
        }

     
        public async Task<ResponseDto> RemoveFromCartAync(int cartDetailsId)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDetailsId,
                Url = StaticDetails.ShoppingCartApiBaseUrl + $"/api/cart/RemoveCart"
            });
            return resp;
        }              

        public async Task<ResponseDto> UpsertCartAsync(CartDto dto)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url = StaticDetails.ShoppingCartApiBaseUrl + $"/api/cart/CartUpsert"
            });
            return resp;
        }
    }
}
