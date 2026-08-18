using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("LaboratoryMessagesReport")]
public partial class LaboratoryMessagesReport
{
    [Key]
    public long Id { get; set; }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [Required]
    [StringLength(15)]
    public string Mobile { get; set; }

    [StringLength(255)]
    public string NameLab { get; set; }

    [Required]
    [StringLength(1024)]
    public string PdfUrl { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Cost { get; set; }

    [Column("result")]
    public bool Result { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    public long CreateBy { get; set; }

    [ForeignKey("CreateBy")]
    [InverseProperty("LaboratoryMessagesReports")]
    public virtual User CreateByNavigation { get; set; }
}
