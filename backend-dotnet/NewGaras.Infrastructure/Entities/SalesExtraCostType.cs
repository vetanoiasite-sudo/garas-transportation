using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesExtraCostType")]
public partial class SalesExtraCostType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [InverseProperty("ExtraCostType")]
    public virtual ICollection<SalesOfferExtraCost> SalesOfferExtraCosts { get; set; } = new List<SalesOfferExtraCost>();
}
