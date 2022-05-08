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
		APIControllerBase
	{
		private readonly IInquiryService _inquiryService;

		public InquiryController(IInquiryService inquiryService, IConfiguration configuration)
			: base(configuration)
		{
			_inquiryService = inquiryService ?? throw new ArgumentNullException(nameof(inquiryService));
		}

		[HttpGet]
		public async Task<ActionResult<List<InquiryDTO>>> GetInquiries()
		{
			var dtos = await _inquiryService.GetAllAsync();
			return Ok(dtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<InquiryDTO>> GetInquiry(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				var dto = await _inquiryService.GetByIdAsync(id);
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

		[HttpPost]
		public async Task<ActionResult> PostInquiry(PostInquiryDTO dto)
		{
			try
			{
				await _inquiryService.AddAsync(dto);

				return StatusCode(201);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutInquiry(string id, PostInquiryDTO dto)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				if (!await _inquiryService.ExistsByIdAsync(id))
				{
					return BadRequest();
				}

				await _inquiryService.UpdateAsync(dto, id);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteInquiry(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				return BadRequest();
			}

			try
			{
				if (!await _inquiryService.ExistsByIdAsync(id))
				{
					return NotFound();
				}

				await _inquiryService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
