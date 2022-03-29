using GT.Core.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	/// <summary>
	/// Represents an ExperienceLevel view and create model.
	/// </summary>
	public class ExperienceLevelDTO : IGTDataTransferObject
	{
		[StringLength(450)]
		public string Id { get; set; }

		[StringLength(100)]
		public string Name { get; set; }
	}
}
