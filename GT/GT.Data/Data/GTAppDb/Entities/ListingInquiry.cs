using GT.Data.Data.GTIdentityDb.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Data.Data.GTAppDb.Entities
{
	internal class ListingInquiry : IGTEntity
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public string MessageTitle { get; set; }

		[Column(TypeName = "nvarchar(500)")]
		public string MessageBody { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public string? LinkedInLink { get; set; }

		public ApplicationUser? UserApplicant { get; set; }
	}
}