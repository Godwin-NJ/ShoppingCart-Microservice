using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mongo.Service.AuthApi.Models;
using Mongo.Service.AuthApi.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mongo.Service.AuthApi.Service
{
	public class JwtTokenGenerator : IJwtTokenGenerator
	{
		private readonly JwtOptions _jwtOptions;
		
        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;			
        }
        public string GenerateToken(ApplicationUser dto, IEnumerable<string> roles)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes(_jwtOptions.secret);

			List<Claim> claims = new List<Claim> 
			{
				// i can use either cliamTypes or jwtRegisteredNames , very flexible
				new Claim(JwtRegisteredClaimNames.Name, dto.UserName),
				new Claim(JwtRegisteredClaimNames.Sub, dto.Id),
				new Claim(JwtRegisteredClaimNames.Email, dto.Email),
			};

			claims.AddRange(roles.Select( role => new Claim(ClaimTypes.Role, role)));

			//new JwtSecurityToken

			var tokenDescriptor = new SecurityTokenDescriptor()
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);

		}
	}
}
