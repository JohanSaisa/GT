using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.AppDb.Entities
{
	public class Location : IAppEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; } = string.Empty;

		[Column(TypeName = "nvarchar(200)")]
		public string? Name { get; set; }

		public ICollection<Company> Companies { get; set; } = new List<Company>();
	}
}
