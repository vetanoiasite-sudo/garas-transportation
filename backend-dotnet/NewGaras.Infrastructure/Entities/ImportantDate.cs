using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ImportantDate")]
public partial class ImportantDate
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReminderDate { get; set; }

    public string Comment { get; set; }

    [Required]
    [StringLength(100)]
    public string Status { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Required]
    [StringLength(100)]
    public string Type { get; set; }

    public string FilePath { get; set; }

    [StringLength(250)]
    public string FileName { get; set; }

    [StringLength(50)]
    public string Fileextention { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ImportantDates")]
    public virtual User CreatedByNavigation { get; set; }
}
