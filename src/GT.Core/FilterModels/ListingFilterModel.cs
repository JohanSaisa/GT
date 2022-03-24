namespace GT.Core.FilterModels
{
	public class ListingFilterModel : IListingFilterModel
	{
		//Includes listing body and title
		public string FreeText { get; set; }
		public int MinSalary { get; set; }
		public int MaxSalary { get; set; }

	}
}
