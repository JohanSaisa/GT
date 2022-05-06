using GT.Core.DTO.Inquiry;
using GT.Core.Services.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GT.API.Controllers
{
	[Route("api/v1/inquiries")]
	[ApiController]
	public class InquiryController :
		GTControllerBase
	{
		private readonly IGTListingInquiryService _inquiryService;

		public InquiryController(IGTListingInquiryService inquiryService, IConfiguration configuration)
			: base(configuration)
		{
			_inquiryService = inquiryService ?? throw new ArgumentNullException(nameof(inquiryService));
		}

		// GET: /overview
		[Route("overview")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<string>>> GetInquiries()
		{
			var dtos = await _inquiryService.GetAllAsync();

			if (dtos is null)
			{
				return NotFound();
			}

			var result = JsonConvert.SerializeObject(dtos);

			return Ok(result);
		}

		// GET: /5
		[HttpGet("{id}")]
		public async Task<ActionResult<string>> GetInquiry(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var dto = await _inquiryService.GetByIdAsync(id);

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
		public async Task<ActionResult<string>> PostInquiry(InquiryDTO dto)
		{
			if (dto is null)
			{
				return BadRequest();
			}

			try
			{
				var objToReturn = await _inquiryService.AddAsync(dto, GetUserId());

				var result = JsonConvert.SerializeObject(objToReturn);

				return StatusCode(201, result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		// PUT: update/5
		[Route("update/{id}")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutInquiry(string id, InquiryDTO dto)
		{
			if (string.IsNullOrEmpty(id) || id != dto.Id)
			{
				return BadRequest();
			}

			try
			{
				await _inquiryService.UpdateAsync(dto, id);
				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		// DELETE: delete/5
		[Route("delete/{id}")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteInquiry(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return BadRequest();
			}

			var inquiryExists = await _inquiryService.ExistsByIdAsync(id);
			if (!inquiryExists)
			{
				return NotFound();
			}

			try
			{
				await _inquiryService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}
