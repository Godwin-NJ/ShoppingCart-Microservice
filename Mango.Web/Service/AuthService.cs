using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
	public class AuthService : IAuthService
	{
		private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto dto)
		{
			var resp = await _baseService.SendAsync(new RequestDto()
			{
				ApiType = StaticDetails.ApiType.POST,
				Data = dto,
				Url = StaticDetails.AuthApiBaseUrl + $"/api/auth/assignrole"
			});
			return resp;
		}

		public async Task<ResponseDto?> LoginAsync(LoginRequestDto dto)
		{
			var resp = await _baseService.SendAsync(new RequestDto()
			{
				ApiType = StaticDetails.ApiType.POST,
				Data = dto,
				Url = StaticDetails.AuthApiBaseUrl + $"/api/auth/login"
			},withBearer:false);
			return resp;
		}

		public async Task<ResponseDto?> RegistrationAsync(RegistrationRequestDto dto)
		{
			var resp = await _baseService.SendAsync(new RequestDto()
			{
				ApiType = StaticDetails.ApiType.POST,
				Data = dto,
				Url = StaticDetails.AuthApiBaseUrl + $"/api/auth/register"
			}, withBearer: false);
			return resp;
		}
	}
}
