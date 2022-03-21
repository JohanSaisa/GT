using GT.Data.Data.GTIdentityDb.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Data.Data.GTAppDb.Entities
{
  internal class Listing : IGTEntity
  {
    [Column(TypeName = "nvarchar(450)")]
    public string Id { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string ListingTitle { get; set; }

    [Column(TypeName = "nvarchar(2000)")]
    public string? Description { get; set; }

    public Company? Employer { get; set; }
    public int? SalaryMin { get; set; }
    public int? SalaryMax { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string? JobTitle { get; set; }

    public Address? Location { get; set; }

    //Full Time Equivelent
    public bool? FTE { get; set; }

    public ApplicationUser? CreatedBy { get; set; }

    [Column(TypeName = "Date")]
    public DateTime CreatedDate { get; set; }

    public ICollection<ListingInquiry> Inquiries { get; set; }
  }
}