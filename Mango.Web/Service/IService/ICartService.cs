using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserIdAsync(string UserId);
        Task<ResponseDto> UpsertCartAsync(CartDto dto);
        Task<ResponseDto> RemoveFromCartAync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto dto);
      
    }
}
