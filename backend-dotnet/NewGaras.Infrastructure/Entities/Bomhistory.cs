using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOMHistory")]
public partial class Bomhistory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMID")]
    public long Bomid { get; set; }

    [Column("BOMManHoursCost", TypeName = "decimal(18, 4)")]
    public decimal BommanHoursCost { get; set; }

    [Column("BOMMaterialCost", TypeName = "decimal(18, 4)")]
    public decimal BommaterialCost { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("Bomid")]
    [InverseProperty("Bomhistories")]
    public virtual Bom Bom { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BomhistoryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BomhistoryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
