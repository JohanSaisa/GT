using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.ExperienceLevel
{
	public class PostExperienceLevelDTO
	{
		[Required(ErrorMessage = "Name is required")]
		[StringLength(100)]
		public string? Name { get; set; }
	}
}
