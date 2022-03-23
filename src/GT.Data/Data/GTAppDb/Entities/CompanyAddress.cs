using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Data.Data.GTAppDb.Entities
{
	public class AddressCompany
	{
		[Column(TypeName = "nvarchar(450)")]
		public string Id { get; set; }
		public string CompanyId { get; set; }
		public string AddressId { get; set; }
	}
}
