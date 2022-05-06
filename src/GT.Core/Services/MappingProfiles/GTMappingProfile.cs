using AutoMapper;
using GT.Core.DTO.Company;
using GT.Core.DTO.Impl;
using GT.Core.DTO.Listing;
using GT.Data.Data.GTAppDb.Entities;

namespace GT.Core.Services.MappingProfiles
{
	internal class GTMappingProfile : Profile
	{
		public GTMappingProfile()
		{
			CreateMap<Company, CompanyDTO>()
				.ReverseMap();

			CreateMap<Listing, ListingOverviewDTO>()
				.ReverseMap();

			CreateMap<ListingInquiry, InquiryDTO>()
				.ReverseMap();

			CreateMap<Location, LocationDTO>()
				.ReverseMap();
		}
	}
}
