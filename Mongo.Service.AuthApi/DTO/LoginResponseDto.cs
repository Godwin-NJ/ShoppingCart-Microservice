namespace Mongo.Service.AuthApi.DTO
{
	public class LoginResponseDto
	{
		public UserDto User { get; set; }
		public string JwtToken { get; set; }
	}
}
