using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOMPartitionHistory")]
public partial class BompartitionHistory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMPartitionID")]
    public long BompartitionId { get; set; }

    [Column("BOMPartitionManHoursCost", TypeName = "decimal(18, 4)")]
    public decimal BompartitionManHoursCost { get; set; }

    [Column("BOMPartitionMaterialCost", TypeName = "decimal(18, 4)")]
    public decimal BompartitionMaterialCost { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("BompartitionId")]
    [InverseProperty("BompartitionHistories")]
    public virtual Bompartition Bompartition { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BompartitionHistoryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BompartitionHistoryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
