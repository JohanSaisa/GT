﻿using AutoMapper;
using GT.Core.DTO;
using GT.Data.Data.GTAppDb.Entities;

namespace GT.Core.Services.MappingProfiles
{
	internal class GTMappingProfile : Profile
	{
		public GTMappingProfile()
		{
			CreateMap<Company, CompanyDTO>()
				.ReverseMap();

			CreateMap<Listing, ListingDTO>()
				.ReverseMap();

			CreateMap<ListingInquiry, ListingInquiryDTO>()
				.ReverseMap();

			CreateMap<Location, LocationDTO>()
			.ReverseMap();
		}
	}
}