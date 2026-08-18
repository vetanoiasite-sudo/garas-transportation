using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BOMProduct")]
public partial class Bomproduct
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("BOMID")]
    public long Bomid { get; set; }

    [Column("ProductGroupID")]
    public int ProductGroupId { get; set; }

    [Column("ProductID")]
    public long ProductId { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("Bomid")]
    [InverseProperty("Bomproducts")]
    public virtual Bom Bom { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BomproductCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BomproductModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Bomproducts")]
    public virtual InventoryItem Product { get; set; }

    [ForeignKey("ProductGroupId")]
    [InverseProperty("Bomproducts")]
    public virtual InventoryItemCategory ProductGroup { get; set; }
}
