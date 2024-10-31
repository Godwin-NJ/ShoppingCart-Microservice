using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly ITokenProvider _tokenProvider;
		public AuthController(IAuthService authService, ITokenProvider tokenProvider)
		{
			_authService = authService;
			_tokenProvider = tokenProvider;
		}

		[HttpGet]
        public IActionResult Login()
		{
			LoginRequestDto requestDto = new();
			return View(requestDto);
		}

		[HttpPost]		
        public async Task<IActionResult> Login(LoginRequestDto dto)
		{
			ResponseDto respDto = await _authService.LoginAsync(dto);
			if(respDto != null && respDto.IsSuccess)
			{
				LoginResponseDto loginResponseDto = 
					JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(respDto.Result));


				await SignInUser(loginResponseDto);
				_tokenProvider.SetToken(loginResponseDto.JwtToken);

				return RedirectToAction("Index", "Home");
			}
			else
			{
				TempData["error"] = respDto.Message;
				return View(dto);
			}
			//else
			//{
			//	ModelState.AddModelError("CustomError", respDto.Message);
			//	return View(dto);
			//}

		}
		
		[HttpGet]
        public IActionResult Register()
		{
			var roleList = new List<SelectListItem>()
			{
				new SelectListItem{Text=StaticDetails.RoleAdmin , Value=StaticDetails.RoleAdmin},
				new SelectListItem{Text=StaticDetails.RoleCustomer , Value=StaticDetails.RoleCustomer},
			};

			ViewBag.RoleList = roleList;
		
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto dto)
        {
			ResponseDto registerUser = await _authService.RegistrationAsync(dto);

			ResponseDto AssignRole;

			if (registerUser != null && registerUser.IsSuccess) 
			{
				if (string.IsNullOrEmpty(dto.Role))
				{
					dto.Role = StaticDetails.RoleCustomer;
				}
				AssignRole = await _authService.AssignRoleAsync(dto);

				if(AssignRole != null || AssignRole.IsSuccess)
				{
					TempData["success"] = "Registration Successful";
					return RedirectToAction(nameof(Login));
				}
			}
			else
			{
				TempData["error"] = registerUser.Message;
			}

            var roleList = new List<SelectListItem>()
            {
                new SelectListItem{Text=StaticDetails.RoleAdmin , Value=StaticDetails.RoleAdmin},
                new SelectListItem{Text=StaticDetails.RoleCustomer , Value=StaticDetails.RoleCustomer},
            };

            ViewBag.RoleList = roleList;

            return View(dto);
        }


        public async  Task<IActionResult> Logout()
		{
		 await	HttpContext.SignOutAsync();
				_tokenProvider.ClearToken();

			return RedirectToAction("Index", "Home");
		}


		private async Task SignInUser(LoginResponseDto model)
		{
			var handler = new JwtSecurityTokenHandler();

			var jwt = handler.ReadJwtToken(model.JwtToken);

			var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
				jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
				jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub).Value));

			identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
				jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name).Value));

			identity.AddClaim(new Claim(ClaimTypes.Name,
				jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(x => x.Type == "role").Value));


            var principle = new ClaimsPrincipal(identity);
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
		}
	}
}
