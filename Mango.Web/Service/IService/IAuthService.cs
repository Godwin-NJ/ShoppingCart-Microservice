using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
	public interface IAuthService
	{
		Task<ResponseDto?> LoginAsync(LoginRequestDto dto);
		Task<ResponseDto?> RegistrationAsync(RegistrationRequestDto dto);
		Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto dto);

	}
}
