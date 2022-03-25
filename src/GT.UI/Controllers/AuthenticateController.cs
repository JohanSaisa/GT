﻿using GT.Data.Data.GTIdentityDb.Entities;
using GT.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticateController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;

		public AuthenticateController(
			SignInManager<ApplicationUser> signInManager,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IConfiguration configuration)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
		}

		[HttpGet]
		public async Task<IActionResult> TestToken()
		{
			if (_signInManager.IsSignedIn(User))
			{
				var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
				var userName = User.FindFirstValue(ClaimTypes.Name); // will give the user's userName
				var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's Email

				var token = User.FindFirstValue(ClaimTypes.Rsa);

				return Ok($"User ID: {userId}, Username: {userName}, Email: {userEmail}, token: {token}");
			}

			return Unauthorized();
		}

		[HttpPost]
		public async Task<IActionResult> Login([FromBody] LoginViewModel loginModel)
		{
			var user = await _userManager.FindByNameAsync(loginModel.Username);
			if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
			{
				var userRoles = await _userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var token = GetToken(authClaims);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo,
				});
			}

			return Unauthorized();
		}

		private JwtSecurityToken GetToken(List<Claim> authClaims)
		{
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

			var token = new JwtSecurityToken(
					issuer: _configuration["JWT:ValidIssuer"],
					audience: _configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(3),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
					);

			return token;
		}
	}
}
