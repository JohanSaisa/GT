using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTAppDb.Entities
{
	public class Company : IGTEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; }

		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }

		public ICollection<Location>? Locations { get; set; }

		[Column(TypeName = "nvarchar(450)")]
		public string? CompanyLogoId { get; set; }
	}
}
