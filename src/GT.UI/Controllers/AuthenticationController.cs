using GT.Data.Data.GTIdentityDb.Entities;
using GT.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthenticationController(
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration configuration)
		{
			_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
			_roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		[HttpPost]
		public async Task<IActionResult> CreateToken([FromBody] JwtTokenViewModel loginModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(loginModel.Username);
				var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
				if (signInResult.Succeeded)
				{
					var userRoles = await _userManager.GetRolesAsync(user);
					var claims = new List<Claim>()
					{
						new Claim(JwtRegisteredClaimNames.Sub, loginModel.Username),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(JwtRegisteredClaimNames.UniqueName, loginModel.Username),
					};

					foreach (var userRole in userRoles)
					{
						claims.Add(new Claim(ClaimTypes.Role, userRole));
					}

					var token = GetToken(claims);

					var results = new
					{
						token = new JwtSecurityTokenHandler().WriteToken(token),
						expiration = token.ValidTo
					};

					return Created("", results);
				}
				else
				{
					return Unauthorized();
				}
			}

			return BadRequest();
		}

		private JwtSecurityToken GetToken(List<Claim> authClaims)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[GTJwtConstants.Key]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
						issuer: _configuration[GTJwtConstants.Issuer],
						audience: _configuration[GTJwtConstants.Audience],
						claims: authClaims,
						expires: DateTime.UtcNow.AddMinutes(60),
						signingCredentials: credentials
						);

			return token;
		}
	}
}
