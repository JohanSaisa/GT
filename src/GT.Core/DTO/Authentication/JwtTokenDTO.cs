using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Impl
{
	public class JwtTokenDTO
	{
		[Required(ErrorMessage = "User name is required")]
		public string? Username { get; set; }

		[Required(ErrorMessage = "User name is required")]
		public string? Password { get; set; }
	}
}
