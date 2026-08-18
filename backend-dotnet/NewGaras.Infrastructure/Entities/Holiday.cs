using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Holiday")]
public partial class Holiday
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Descreption { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [Required]
    [StringLength(250)]
    public string ModifiedBy { get; set; }

    [InverseProperty("Holiday")]
    public virtual ICollection<OffDay> OffDays { get; set; } = new List<OffDay>();
}
