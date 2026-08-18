using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class InsuranceCompanyName
{
    [Key]
    public long Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; }

    public bool? Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreationDate { get; set; }

    public long? CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InsuranceCompanyNames")]
    public virtual User CreatedByNavigation { get; set; }
}
