using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InvoiceOfProject")]
public partial class InvoiceOfProject
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InvoiceID")]
    public long InvoiceId { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("InvoiceOfProjects")]
    public virtual Project Project { get; set; }
}
