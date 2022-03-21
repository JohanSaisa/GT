using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.Data.Data.GTAppDb.Entities
{
  internal class Company : IGTEntity
  {
    [Column(TypeName = "nvarchar(450)")]
    public string Id { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string Name { get; set; }

    public ICollection<Address> Addresses { get; set; }
  }
}