﻿using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GT.API.Controllers
{
	[Route("api/v1/listings")]
	[ApiController]
	public class ListingController : ControllerBase
	{
		private readonly IGTListingService _listingService;
		private readonly IGTExperienceLevelService _experienceService;
		private readonly IGTLocationService _locationService;
		private readonly IGTListingInquiryService _listingInquiryService;
		private readonly IConfiguration _configuration;

		public ListingController(
			IGTListingService listingService,
			IGTExperienceLevelService experienceService,
			IGTLocationService locationService,
			IGTListingInquiryService listingInquiryService,
			IConfiguration configuration)
		{
			_listingService = listingService ?? throw new ArgumentNullException(nameof(listingService));
			_experienceService = experienceService ?? throw new ArgumentNullException(nameof(experienceService));
			_locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
			_listingInquiryService = listingInquiryService ?? throw new ArgumentNullException(nameof(listingInquiryService));
			_configuration = configuration;
		}

		// GET: /overview
		[Route("overview")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetListingsWithFilter()
		{
			ListingFilterModel filterModel = new ListingFilterModel();

			var listingDTOs = await _listingService
				.GetAsync(filterModel);

			if (listingDTOs == null)
			{
				return NotFound();
			}
			var result = JsonConvert.SerializeObject(listingDTOs);

			return Ok(result);
		}

		// GET:/5
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetListing(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var listing = await _listingService
				.GetByIdAsync(id);

			if (listing == null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(listing);

			return Ok(result);
		}

		// GET: delete/5
		[Route("delete")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteListing(string? id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				await _listingService.DeleteAsync(id);
			}
			catch (Exception)
			{
				return NotFound();
			}

			return Ok();
		}

		// PUT: update/5
		[Route("update")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutListing(string id, ListingDTO listing)
		{
			if (string.IsNullOrEmpty(id) || id != listing.Id)
			{
				return BadRequest();
			}

			try
			{
				await _listingService.UpdateAsync(listing, id);
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// POST: /create
		[Route("create")]
		[HttpPost]
		public async Task<ActionResult<string?>> PostListing(ListingDTO listing)
		{
			// TODO Need to learn how to get user id from jwt

			if (listing is null)
			{
				return BadRequest();
			}

			try
			{
				string authHeaderValue = Request.Headers["Authorization"];

				var tokenClaims = GetClaims(authHeaderValue.Substring("Bearer ".Length).Trim());

				var userId = tokenClaims.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

				var objToReturn = await _listingService.AddAsync(listing, GetUserId());
				var result = JsonConvert.SerializeObject(objToReturn);
				return Ok(result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		private string GetUserId()
		{
			return User.Claims
				.First(i => i.Type == "Id").Value;
		}

		public ClaimsPrincipal GetClaims(string token)
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

			return handler.ValidateToken(token, validations, out var tokenSecure);
		}
	}
}
