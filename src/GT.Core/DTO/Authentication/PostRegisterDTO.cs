using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Authentication
{
	public class PostRegisterDTO
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string? Password { get; set; }
	}
}
