using GT.Core.FilterModels.Impl;
using GT.Core.Services.Impl;
using GT.Data.Data.GTAppDb.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestListingsController : ControllerBase
	{
		private readonly IGTListingService _listingService;

		public TestListingsController(IGTListingService listingService)
		{
			_listingService = listingService;
		}

		// GET: api/<TestListingsController>
		[HttpGet]
		public async Task<IEnumerable<Listing>> Get()
		{
			var filter = new ListingFilterModel()
			{
				ExperienceLevel = new List<string>
				{
					"Junior",
					"Banan"
				},
				//FreeText = new List<string>
				//{
				//	"Stockholm",
				//	"Banan"
				//},
			};

			return await _listingService.GetAsync(filter);
		}
	}
}
