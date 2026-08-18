using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryMatrialRequest")]
public partial class InventoryMatrialRequest
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("FromUserID")]
    public long FromUserId { get; set; }

    [Column("ToInventoryStoreID")]
    public int ToInventoryStoreId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RequestDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("RequestTypeID")]
    public long? RequestTypeId { get; set; }

    [StringLength(50)]
    public string ApproveResult { get; set; }

    public string ApproveRejectNotes { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("InventoryMatrialRequestCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("FromUserId")]
    [InverseProperty("InventoryMatrialRequestFromUsers")]
    public virtual User FromUser { get; set; }

    [InverseProperty("MaterialRequest")]
    public virtual ICollection<Hrcustody> Hrcustodies { get; set; } = new List<Hrcustody>();

    [InverseProperty("MatrialRequest")]
    public virtual ICollection<InventoryMatrialRelease> InventoryMatrialReleases { get; set; } = new List<InventoryMatrialRelease>();

    [InverseProperty("InventoryMatrialRequest")]
    public virtual ICollection<InventoryMatrialRequestItem> InventoryMatrialRequestItems { get; set; } = new List<InventoryMatrialRequestItem>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("InventoryMatrialRequestModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("MatrialRequest")]
    public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; } = new List<PurchaseRequest>();

    [ForeignKey("RequestTypeId")]
    [InverseProperty("InventoryMatrialRequests")]
    public virtual InventoryMaterialRequestType RequestType { get; set; }

    [ForeignKey("ToInventoryStoreId")]
    [InverseProperty("InventoryMatrialRequests")]
    public virtual InventoryStore ToInventoryStore { get; set; }
}
