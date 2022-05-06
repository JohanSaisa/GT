using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using GT.Core.DTO.Company;
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
		private readonly ICompanyService _companyService;

		public CompanyController(
			ICompanyService companyService)
		{
			_companyService = companyService;
		}

		// GET:
		[HttpGet()]
		public async Task<ActionResult<List<CompanyDTO>>> GetAllCompanies()
		{
			try
			{
				var dtos = await _companyService.GetAllAsync();

				return Ok(dtos);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// GET: /5
		[HttpGet("{id}")]
		public async Task<ActionResult<CompanyDTO>> GetCompany(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				var dto = await _companyService.GetByIdAsync(id);

				if (dto is null)
				{
					return NotFound();
				}

				return Ok(dto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// POST
		[HttpPost]
		public async Task<ActionResult> PostCompany(PostCompanyDTO dto)
		{
			try
			{
				if (!await _companyService.AddAsync(dto))
				{
					return StatusCode(500, "Could not save the company.");
				}

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		// PUT: /5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCompany(string id, PostCompanyDTO dto)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				await _companyService.UpdateAsync(dto, id);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			return Ok();
		}

		// DELETE: /5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{

				if (!await _companyService.DeleteAsync(id))
				{
					return StatusCode(500, "Could not delete the company.");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

			return Ok();
		}

		// TODO: Create controller for handling company images.
	}
}
