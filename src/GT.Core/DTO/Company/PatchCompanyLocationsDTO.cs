﻿using GT.Core.DTO.Impl;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Company
{
	/// <summary>
	/// Represents a Company view and create model.
	/// </summary>
	public class PatchCompanyLocationsDTO
	{
		[Required(ErrorMessage = "Locations is required")]
		public List<LocationDTO>? Locations { get; set; }
	}
}