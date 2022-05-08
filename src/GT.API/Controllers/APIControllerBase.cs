using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace GT.API.Controllers;

public abstract class APIControllerBase : ControllerBase
{
	protected readonly IConfiguration _configuration;

	protected APIControllerBase(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	protected string GetSignedInUserId()
	{
		string authHeaderValue = Request.Headers["Authorization"];

		var tokenClaims = GetClaims(authHeaderValue.Substring("Bearer ".Length).Trim());

		var signedInUserId = tokenClaims.Claims
			.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

		if (signedInUserId is null)
		{
			throw new ArgumentException(nameof(signedInUserId), $"Could not retrieve the signed in user id.");
		}

		return signedInUserId;
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
