using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GT.API.Controllers;

public abstract class GTControllerBase : ControllerBase
{
	protected readonly IConfiguration _configuration;

	protected GTControllerBase(IConfiguration configuration)
	{
		_configuration = configuration;
	}
	
	protected string GetUserId()
	{
		string authHeaderValue = Request.Headers["Authorization"];

		var tokenClaims = GetClaims(authHeaderValue.Substring("Bearer ".Length).Trim());

		var userId = tokenClaims.Claims
			.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

		return userId;
	}
	
	private ClaimsPrincipal GetClaims(string token)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:Jwt:Key"]));
		var handler = new JwtSecurityTokenHandler();
		var validations = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = key,
			ValidateIssuer = false,
			ValidateAudience = false
		};

		return handler.ValidateToken(token, validations, out _);
	}
}
