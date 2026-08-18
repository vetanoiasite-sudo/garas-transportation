using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryReportItemParent")]
public partial class InventoryReportItemParent
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("InvReportItemID")]
    public long InvReportItemId { get; set; }

    [Column("InvStoreItemID")]
    public long InvStoreItemId { get; set; }

    public bool Finished { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("InvReportItemId")]
    [InverseProperty("InventoryReportItemParents")]
    public virtual InventoryReportItem InvReportItem { get; set; }

    [ForeignKey("InvStoreItemId")]
    [InverseProperty("InventoryReportItemParents")]
    public virtual InventoryStoreItem InvStoreItem { get; set; }
}
