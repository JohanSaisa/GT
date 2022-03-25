using System.ComponentModel.DataAnnotations;

namespace GT.UI.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "User name is required")]
		public string? Username { get; set; }

		[Required(ErrorMessage = "User name is required")]
		public string? Password { get; set; }
	}
}
