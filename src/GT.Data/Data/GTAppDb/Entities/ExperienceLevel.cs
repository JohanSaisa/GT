﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GT.Data.Data.GTAppDb.Entities
{
	[Table("ExperienceLevel")]
	public class ExperienceLevel : IGTEntity
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public ICollection<Listing>? Listings { get; set; }
	}
}
