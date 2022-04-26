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

		public CompanyController(
			IGTCompanyService companyService)
		{
			_companyService = companyService;
		}

		// GET: /overview
		[Route("overview")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetAllCompanies()
		{
			var dtos = await _companyService
				.GetAllAsync();

			if (dtos is null)
			{
				return StatusCode(500);
			}

			var result = JsonConvert.SerializeObject(dtos);

			return Ok(result);
		}

		// GET: /5
		[Route("getbyid/{id}")]
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetCompany(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var dto = await _companyService.GetByIdAsync(id);

			if (dto is null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(dto);

			return Ok(result);
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
