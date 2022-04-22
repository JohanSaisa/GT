using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using GT.Core.DTO.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GT.API.Controllers
{
	[Route("api/v1/companies")]
	[ApiController]
	public class CompanyController : ControllerBase
	{
		private readonly IGTCompanyService _companyService;
		private readonly ILogger<CompanyController> _logger;
		private readonly IConfiguration _configuration;

		public CompanyController(
			IGTCompanyService companyService,
			ILogger<CompanyController> logger,
			IConfiguration configuration)
		{
			_companyService = companyService;
			_logger = logger;
			_configuration = configuration;
		}

		// GET: api/Company
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetAllCompanies()
		{
			var companyDTOs = await _companyService
				.GetAsync();

			if (companyDTOs is null)
			{
				return StatusCode(500);
			}

			var result = JsonConvert.SerializeObject(companyDTOs);

			return Ok(result);
		}

		// GET: /5
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetCompany(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var company = await _companyService
				.GetByIdAsync(id);

			if (company is null)
			{
				return NotFound();
			}
			
			var result = JsonConvert.SerializeObject(company);

			return Ok(result);
		}

		// POST: api/Company
		[Route("create")]
		[HttpPost]
		public async Task<ActionResult<string>> PostCompany(CompanyDTO company)
		{
			if (company is null)
			{
				return BadRequest();
			}

			try
			{
				var objToReturn = await _companyService.AddAsync(company);
				
				var result = JsonConvert.SerializeObject(objToReturn);
				
				return Created("", result);
			}
			catch
			{
				return StatusCode(500);
			}
		}

		// PUT: company/5
		[Route("update")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCompany(string id, CompanyDTO company)
		{
			if (string.IsNullOrEmpty(id) || id != company.Id)
			{
				return BadRequest();
			}

			try
			{
				await _companyService.UpdateAsync(company, id);
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// TODO: Create controller for handling company images.
		
		// DELETE: api/Company/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
