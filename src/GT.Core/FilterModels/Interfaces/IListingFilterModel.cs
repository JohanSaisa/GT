﻿namespace GT.Core.FilterModels.Interfaces
{
	public interface IListingFilterModel : IFilterModel
	{
		// Search result checks the listing's experience level property, 
		// and will include all entities matching at least one value in the array.
		string[]? ExperienceLevel { get; set; }

		// Search result checks the listing's title, description and location properties,
		// and will only include entities where each value in the array is present in at least one of those properties.
		string[]? FreeText { get; set; }

		// First Time Equivalent.
		bool? FTE { get; set; }

		string? Location { get; set; }

		// Search result checks the listing's salary property,
		// and will include entities where the value of that property falls within the range between MinSalary and MaxSalary.
		int? MaxSalary { get; set; }
		int? MinSalary { get; set; }

		// Earliest listing post date.
		public DateTime? PostDate { get; set; }
	}
}