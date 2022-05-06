using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	public class PostLocationDTO
	{
		[Required(ErrorMessage = "Name is required")]
		[StringLength(100)]
		public string? Name { get; set; }
	}
}
