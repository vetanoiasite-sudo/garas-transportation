using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOMPartitions")]
public partial class Bompartition
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMID")]
    public long Bomid { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [StringLength(50)]
    public string Serial { get; set; }

    public string Description { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PartitionWorkingHours { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal PartitionTotalWorkingHoursCost { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("Bomid")]
    [InverseProperty("Bompartitions")]
    public virtual Bom Bom { get; set; }

    [InverseProperty("Bompartition")]
    public virtual ICollection<BompartitionAttachment> BompartitionAttachments { get; set; } = new List<BompartitionAttachment>();

    [InverseProperty("Bompartition")]
    public virtual ICollection<BompartitionHistory> BompartitionHistories { get; set; } = new List<BompartitionHistory>();

    [InverseProperty("Bompartition")]
    public virtual ICollection<BompartitionItem> BompartitionItems { get; set; } = new List<BompartitionItem>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("BompartitionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BompartitionModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
