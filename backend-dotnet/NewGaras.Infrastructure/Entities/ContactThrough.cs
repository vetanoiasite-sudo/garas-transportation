using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ContactThrough")]
public partial class ContactThrough
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string CreationBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [InverseProperty("ContactThrough")]
    public virtual ICollection<PofinalSelecteSupplier> PofinalSelecteSuppliers { get; set; } = new List<PofinalSelecteSupplier>();
}
