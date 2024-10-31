using Microsoft.AspNetCore.Identity;

namespace Mongo.Service.AuthApi.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
