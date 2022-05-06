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
		public async Task<ActionResult<string>> GetCompany(string id)
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

				var result = JsonConvert.SerializeObject(dto);

				return Ok(result);
			}
			catch (Exception)
			{
				throw;
			}
		}

		// POST: /create
		[Route("create")]
		[HttpPost]
		public async Task<ActionResult<string>> PostCompany(CompanyDTO dto)
		{
			if (dto is null)
			{
				return BadRequest();
			}

			try
			{
				var objToReturn = await _companyService.AddAsync(dto);

				var result = JsonConvert.SerializeObject(objToReturn);

				return Created("", result);
			}
			catch
			{
				return StatusCode(500);
			}
		}

		// PUT: update/5
		[Route("update/{id}")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCompany(string id, CompanyDTO dto)
		{
			if (string.IsNullOrEmpty(id) || id != dto.Id)
			{
				return BadRequest();
			}

			try
			{
				await _companyService.UpdateAsync(dto, id);
			}
			catch
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// DELETE: delete/5
		[Route("delete/{id}")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			try
			{
				await _companyService.DeleteAsync(id);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}

			return Ok();
		}

		// TODO: Create controller for handling company images.
	}
}
