using GT.Core.DTO.Impl;
using GT.Core.FilterModels.Impl;
using GT.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GT.UI.Pages.Listings
{
	public class IndexModel : PageModel
	{
		private readonly IGTListingService _listingService;

		[BindProperty(SupportsGet = true)]
		public string? SearchString { get; set; }

		public List<ListingOverviewDTO> ListOverview { get; set; } = new List<ListingOverviewDTO>();

		public IndexModel(IGTListingService listingService)
		{
			_listingService = listingService;
		}

		public async void OnGet()
		{
			var filterModel = new ListingFilterModel();

			if (!string.IsNullOrEmpty(SearchString))
			{
				var keyWords = SearchString.Split(' ').ToList();
				filterModel.Keywords = keyWords;
			}

			ListOverview = await _listingService.GetAsync(filterModel);
		}
	}
}
