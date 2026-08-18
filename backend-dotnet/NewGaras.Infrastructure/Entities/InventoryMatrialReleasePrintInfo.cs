using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("InventoryMatrialReleasePrintInfo")]
public partial class InventoryMatrialReleasePrintInfo
{
    [Key]
    public long Id { get; set; }

    public long InventoryMatrialReleaseId { get; set; }

    [StringLength(450)]
    public string ContactPersonName { get; set; }

    [StringLength(450)]
    public string ContactPersonMobile { get; set; }

    public string ClientAddress { get; set; }

    [StringLength(450)]
    public string ShippingMethod { get; set; }

    public string Comment { get; set; }

    [Column("packagingQTY", TypeName = "decimal(10, 4)")]
    public decimal? PackagingQty { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long? ProjectId { get; set; }

    [ForeignKey("InventoryMatrialReleaseId")]
    [InverseProperty("InventoryMatrialReleasePrintInfos")]
    public virtual InventoryMatrialRelease InventoryMatrialRelease { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("InventoryMatrialReleasePrintInfos")]
    public virtual Project Project { get; set; }
}
