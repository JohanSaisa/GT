using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.AppDb.Entities
{
	public class ExperienceLevel : IAppEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }

		public ICollection<Listing>? Listings { get; set; } = new List<Listing>();
	}
}
