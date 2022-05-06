using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Authentication
{
	public class PostRegisterDTO
	{
		[Required]
		[EmailAddress(ErrorMessage = "Email is required")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string? Password { get; set; }
	}
}
