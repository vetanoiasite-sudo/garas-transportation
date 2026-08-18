using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("HRCustody")]
public partial class Hrcustody
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool IsAssetsType { get; set; }

    [Column("InventoryItemID")]
    public long InventoryItemId { get; set; }

    public long MaterialRequestItemId { get; set; }

    public long MaterialRequestId { get; set; }

    public string Description { get; set; }

    public string SignedMemoAttachmentPath { get; set; }

    [StringLength(50)]
    public string SignedMemoFileName { get; set; }

    [StringLength(50)]
    public string SignedMemoFileExtension { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("HrcustodyCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Custody")]
    public virtual ICollection<HrcustodyReportAttachment> HrcustodyReportAttachments { get; set; } = new List<HrcustodyReportAttachment>();

    [ForeignKey("InventoryItemId")]
    [InverseProperty("Hrcustodies")]
    public virtual InventoryItem InventoryItem { get; set; }

    [ForeignKey("MaterialRequestId")]
    [InverseProperty("Hrcustodies")]
    public virtual InventoryMatrialRequest MaterialRequest { get; set; }

    [ForeignKey("MaterialRequestItemId")]
    [InverseProperty("Hrcustodies")]
    public virtual InventoryMatrialRequestItem MaterialRequestItem { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("HrcustodyModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Hrcustodies")]
    public virtual HrcustodyStatus Status { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("HrcustodyUsers")]
    public virtual User User { get; set; }
}
