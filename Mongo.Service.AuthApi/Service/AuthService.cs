using AutoMapper;
using Mango.Services.AuthApi.Data;
using Microsoft.AspNetCore.Identity;
using Mongo.Service.AuthApi.DTO;
using Mongo.Service.AuthApi.Models;
using Mongo.Service.AuthApi.Service.IService;

namespace Mongo.Service.AuthApi.Service
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IJwtTokenGenerator _jwtTokenGenerator;

		public AuthService(
			AppDbContext db, UserManager<ApplicationUser> userManager, 			
			RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
		{
			_db = db;
			_userManager = userManager;
			_roleManager = roleManager;
			_jwtTokenGenerator = jwtTokenGenerator;
		}

		public async Task<bool> Assignrole(string email, string roleName)
		{
			var user = _db.ApplicationUsers.SingleOrDefault(u => u.Email == email);
			if (user != null)
			{
				if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
				{
					_roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
				}
				await _userManager.AddToRoleAsync(user, roleName);
				return true;
			}
			return false;
		}
		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			var user = _db.ApplicationUsers.SingleOrDefault(u => u.UserName == loginRequestDto.UserName);
			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
			if (!isValid || user == null) 
			{
				return new LoginResponseDto() { User = null, JwtToken = "" };
			}

			UserDto userData = new()
			{
				ID = user.Id,
				Name = user.Name,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber
			};

			var roles = await _userManager.GetRolesAsync(user);
			var token = _jwtTokenGenerator.GenerateToken(user, roles);

			return new LoginResponseDto() { User = userData, JwtToken = token };
		}

		public async Task<string> Register(RegistrationRequestDto dto)
		{
			ApplicationUser newUser = new()
			{
				UserName = dto.Email,
				Email = dto.Email,
				NormalizedEmail = dto.Email.ToUpper(),
				PhoneNumber = dto.PhoneNumber,
				Name = dto.Name,
			};

			try
			{
				var result = await _userManager.CreateAsync(newUser, dto.Password);//ths password is hashed here as well
				if (result.Succeeded)
				{
					var userData = _db.ApplicationUsers.SingleOrDefault(x => x.Email == dto.Email);

					UserDto userInfo = new()
					{
						ID = userData.Id,
						Name = userData.Name,
						Email = userData.Email,
						PhoneNumber	= userData.PhoneNumber
					};

					return "";
					//return userInfo;
				}
				else
				{
					return result.Errors.FirstOrDefault().Description;
				}
			}
			catch (Exception)
			{

				//throw;
			}
			return "Error encoutered";
		}
	}
}
