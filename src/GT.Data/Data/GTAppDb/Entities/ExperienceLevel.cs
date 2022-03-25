namespace GT.Data.Data.GTAppDb.Entities
{
	public class ExperienceLevel
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public ICollection<Listing>? Listings { get; set; }
	}
}
