using Mango.Services.AuthApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo.Service.AuthApi.DTO;
using Mongo.Service.AuthApi.Service.IService;

namespace Mongo.Service.AuthApi.Controllers
{
	//[Route("api/[controller]")]
	[Route("api/auth")]
	[ApiController]
	public class AuthAPIController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly ResponseDto _responseDto;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
			_responseDto = new ResponseDto();
        }


        [HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegistrationRequestDto dto)
		{
			var resp = await _authService.Register(dto);
			if (!string.IsNullOrEmpty(resp))
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = resp;
				return BadRequest(_responseDto);
			}

			return Ok(_responseDto);
		}
		
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
		{
			var resp = await _authService.Login(loginRequestDto);

			if(resp.User == null)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Username or paswword is incorrect";	
				return BadRequest(_responseDto);
			}
			_responseDto.Result = resp;
			return Ok(_responseDto);
		}	
		
		
		[HttpPost("assignrole")]
		public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto dto)
		{
			var resp = await _authService.Assignrole( dto.Email,dto.Role.ToUpper());

			if(!resp)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Message = "Errored encountered";	
				return BadRequest(_responseDto);
			}
			
			return Ok(_responseDto);
		}
	}
}
