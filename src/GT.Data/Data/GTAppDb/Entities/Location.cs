using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTAppDb.Entities
{
	[Table("Location")]
	public class Location : IGTEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }

		public ICollection<Company>? Companies { get; set; }
	}
}
