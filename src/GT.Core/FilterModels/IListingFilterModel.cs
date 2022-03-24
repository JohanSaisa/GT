
namespace GT.Core.FilterModels
{
	public interface IListingFilterModel : IFilterModel
	{
		ICollection<string> ExperienceLevel { get; set; }
		string? FreeText { get; set; }
		bool? FTE { get; set; }
		string? Location { get; set; }
		int? MaxSalary { get; set; }
		int? MinSalary { get; set; }
	}
}
