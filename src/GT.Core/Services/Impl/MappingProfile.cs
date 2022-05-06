using AutoMapper;
using GT.Core.DTO.Company;
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
								.MapFrom(src => src.Locations!.Select(e => new LocationDTO
								{
									Id = e.Id,
									Name = e.Name,
								}).ToList()));
		}
	}
}
