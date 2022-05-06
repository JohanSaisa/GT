using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Authentication
{
	public class PostLoginDTO
	{
		[Required(ErrorMessage = "UserName is required")]
		public string? UserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string? Password { get; set; }
	}
}
