using GT.Core.DTO.Impl;
using GT.Core.Services.Impl;
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
		private readonly GTListingInquiryService _inquiryService;

		public InquiryController(GTListingInquiryService inquiryService, IConfiguration configuration) 
			: base(configuration)
		{
			_inquiryService = inquiryService ?? throw new ArgumentNullException(nameof(inquiryService));
		}
		
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

		[Route("create")]
		[HttpPost]
		public async Task<ActionResult<string>> PostInquiry(ListingInquiryDTO dto)
		{
			if (dto is null)
			{
				return BadRequest();
			}

			try
			{
				var objToReturn = await _inquiryService.AddAsync(dto, GetUserId());
				
				var result = JsonConvert.SerializeObject(objToReturn);
				
				return Ok(result);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[Route("update")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutInquiry(string id, ListingInquiryDTO dto)
		{
			
		}

		[Route("delete")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteInquiry(string id)
		{
			
		}
	}
}
