using GT.Core.DTO.Impl;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Company
{
	/// <summary>
	/// Represents a Company view and create model.
	/// </summary>
	public class PatchCompanyNameDTO
	{
		[Required]
		[StringLength(100)]
		public string? Name { get; set; }
	}
}
