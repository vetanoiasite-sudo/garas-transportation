using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("Product")]
public partial class Product
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(1000)]
    public string Name { get; set; }

    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    [Column("ProductGroupID")]
    public int? ProductGroupId { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<Bomlibrary> Bomlibraries { get; set; } = new List<Bomlibrary>();

    [InverseProperty("Product")]
    public virtual ICollection<BranchProduct> BranchProducts { get; set; } = new List<BranchProduct>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProductCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProductModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProductGroupId")]
    [InverseProperty("Products")]
    public virtual ProductGroup ProductGroup { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<SalesOfferProduct> SalesOfferProducts { get; set; } = new List<SalesOfferProduct>();
}
