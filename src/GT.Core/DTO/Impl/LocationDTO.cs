using GT.Core.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	public class LocationDTO : IGTDataTransferObject
	{
		[StringLength(450)]
		public string? Id { get; set; }

		[StringLength(100)]
		public string? Name { get; set; }
	}
}
