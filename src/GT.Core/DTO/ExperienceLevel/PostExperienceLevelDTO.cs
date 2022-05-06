using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.ExperienceLevel
{
	/// <summary>
	/// Represents an ExperienceLevel view and create model.
	/// </summary>
	public class PostExperienceLevelDTO
	{
		[Required]
		[StringLength(100)]
		public string? Name { get; set; }
	}
}
