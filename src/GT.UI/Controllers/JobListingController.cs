using GT.Data.Data.GTAppDb;
using GT.Data.Data.GTAppDb.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GT.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class JobListingController : ControllerBase
	{
		private readonly ILogger<JobListingController> _logger;
		private readonly GTAppContext _listingService;

		public JobListingController(ILogger<JobListingController> logger, GTAppContext listingService)
		{
			_logger = logger;
			_listingService = listingService;
		}

		// GET: api/<JobListingController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Listing>>> GetListing()
		{
			var listings = await _listingService.Listings.FindAsync();
			if (listings == null)
			{
				return NotFound();
			}
			return Ok(listings);
		}

		// GET api/<JobListingController>/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Listing>> GetListing(string id)
		{
			var listing = await _listingService.Listings.FindAsync(id);
			if (listing == null)
			{
				return NotFound();
			}
			return Ok(listing);
		}

		// POST api/<JobListingController>
		[HttpPost]
		public async Task<ActionResult<Listing>> PostListing(Listing listing)
		{
			_listingService.Listings.Add(listing);
			await _listingService.SaveChangesAsync();
			return CreatedAtAction(nameof(GetListing), new { id = listing.Id }, listing);
		}

		// PUT api/<JobListingController>/5
		[HttpPut("{id}")]
		public void Put(string id, [FromBody] string value)
		{
		}

		// DELETE api/<JobListingController>/5
		[HttpDelete("{id}")]
		public void Delete(string id)
		{
		}
	}
}
