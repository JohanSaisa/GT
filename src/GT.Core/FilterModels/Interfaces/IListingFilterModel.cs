namespace GT.Core.FilterModels.Interfaces
{
	/// <summary>
	/// Represents a model containing paramaters used for filtering against the database.
	/// </summary>
	public interface IListingFilterModel
	{
		/// <summary>
		/// Search result checks the listing's experience level property, 
		/// and will include all entities matching at least one value in the array.
		/// </summary>
		List<string>? ExperienceLevels { get; set; }

		/// <summary>
		/// Search result checks the listing's title, description and location properties,
		/// and will only include entities where each value in the array is present in at least one of those properties.
		/// </summary>
		List<string>? Keywords { get; set; }

		/// <summary>
		/// Full Time Equivalent, where true represents Full Time and false represents part time.
		/// </summary>
		bool? FTE { get; set; }

		/// <summary>
		/// String representation of an arbitrary location. Could be a city name or simply 'Remote Work'.
		/// </summary>
		string? Location { get; set; }

		/// <summary>
		/// Search result checks the listing's salary property,
		/// and will include entities where the value of that property falls within the range between MinSalary and MaxSalary.
		/// </summary>
		int? SalaryMax { get; set; }

		/// <summary>
		/// Search result checks the listing's salary property,
		/// and will include entities where the value of that property falls within the range between MinSalary and MaxSalary.
		/// </summary>
		int? SalaryMin { get; set; }

		/// <summary>
		/// Sets breakpoint at the given date.
		/// </summary>
		public DateTime? IncludeListingsFromDate { get; set; }
	}
}
