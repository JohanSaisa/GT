using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.AppDb.Entities
{
	public class Company : IAppEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }

		public ICollection<Location>? Locations { get; set; } = new List<Location>();
	}
}
