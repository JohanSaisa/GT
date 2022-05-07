﻿using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Authentication
{
	public class PostLoginDTO
	{
		[Required(ErrorMessage = "Username is required")]
		public string? Username { get; set; }

		[Required(ErrorMessage = "Password is required")]
		public string? Password { get; set; }
	}
}
