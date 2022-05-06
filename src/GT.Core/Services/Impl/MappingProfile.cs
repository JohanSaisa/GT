using AutoMapper;
using GT.Core.DTO.Company;
using GT.Core.DTO.ExperienceLevel;
using GT.Core.DTO.Impl;
using GT.Core.DTO.Inquiry;
using GT.Core.DTO.Listing;
using GT.Data.Data.AppDb.Entities;

namespace GT.Core.Services.Impl
{
	internal class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Company, CompanyDTO>()
				.ForMember(dest => dest.Locations, options => options
					.MapFrom(src => src.Locations!.Select(e => e.Name).ToList()));

			CreateMap<ExperienceLevel, ExperienceLevelDTO>();

			CreateMap<Inquiry, InquiryDTO>();

			CreateMap<Listing, ListingDTO>()
				.ForMember(dest => dest.Employer, options => options
					.MapFrom(src => src.Employer!.Name))
				.ForMember(dest => dest.Location, options => options
					.MapFrom(src => src.Location!.Name))
				.ForMember(dest => dest.ExperienceLevel, options => options
					.MapFrom(src => src.ExperienceLevel!.Name));

			CreateMap<Location, LocationDTO>();
		}
	}
}
