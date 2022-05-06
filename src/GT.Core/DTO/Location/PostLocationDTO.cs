using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	public class PostLocationDTO
	{
		[StringLength(100)]
		public string? Name { get; set; }
	}
}
