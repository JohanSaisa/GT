﻿using System.ComponentModel.DataAnnotations;

namespace GT.Core.DTO.Inquiry
{
	/// <summary>
	/// Represents a ListingInquiry view and create model.
	/// </summary>
	public class InquiryDTO
	{
		public string? Id { get; set; }

		public string? ListingId { get; set; }

		public string? ApplicantEmail { get; set; }

		public string? MessageTitle { get; set; }

		public string? MessageBody { get; set; }

		public string? LinkedInLink { get; set; }
	}
}
