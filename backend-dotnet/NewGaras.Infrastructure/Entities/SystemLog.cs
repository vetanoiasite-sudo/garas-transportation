using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SystemLog")]
public partial class SystemLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [StringLength(500)]
    public string ActionName { get; set; }

    public int? TableId { get; set; }

    [StringLength(500)]
    public string TableName { get; set; }

    [StringLength(500)]
    public string ColumnName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LogDate { get; set; }

    [StringLength(1000)]
    public string OldValue { get; set; }

    [StringLength(1000)]
    public string NewValue { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SystemLogs")]
    public virtual User CreatedByNavigation { get; set; }
}
