using Mongo.Service.AuthApi.DTO;

namespace Mongo.Service.AuthApi.Service.IService
{
	public interface IAuthService
	{
		Task<string> Register(RegistrationRequestDto registerRequestDto);
		Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
		Task<bool> Assignrole(string email, string roleName);
	}
}
