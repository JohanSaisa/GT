using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTAppDb.Entities
{
	[Table("ExperienceLevels")]
	public class ExperienceLevel : IGTEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }

		public ICollection<Listing>? Listings { get; set; }
	}
}
