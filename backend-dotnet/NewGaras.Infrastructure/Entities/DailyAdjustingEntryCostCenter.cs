using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyAdjustingEntryCostCenter")]
public partial class DailyAdjustingEntryCostCenter
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DailyAdjustingEntryID")]
    public long? DailyAdjustingEntryId { get; set; }

    [Column("CostCenterID")]
    public long? CostCenterId { get; set; }

    [StringLength(50)]
    public string Type { get; set; }

    [Column("TypeID")]
    public long? TypeId { get; set; }

    [Column("ProductID")]
    public long? ProductId { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    [Column("_Quantity")]
    public double? Quantity { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    public string EntryType { get; set; }

    [Column("Quantity", TypeName = "decimal(18, 4)")]
    public decimal? Quantity1 { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DailyAdjustingEntryCostCenterCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DailyAdjustingEntryCostCenterModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
