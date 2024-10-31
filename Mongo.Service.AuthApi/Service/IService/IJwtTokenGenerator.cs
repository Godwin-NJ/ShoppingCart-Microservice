using Mongo.Service.AuthApi.Models;

namespace Mongo.Service.AuthApi.Service.IService
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(ApplicationUser dto, IEnumerable<string> roles);
	}
}
