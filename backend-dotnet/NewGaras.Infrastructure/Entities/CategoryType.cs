using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("CategoryType")]
public partial class CategoryType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string Name { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("CategoryTypeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("CategoryType")]
    public virtual ICollection<InventoryItemCategory> InventoryItemCategories { get; set; } = new List<InventoryItemCategory>();

    [ForeignKey("ModifedBy")]
    [InverseProperty("CategoryTypeModifedByNavigations")]
    public virtual User ModifedByNavigation { get; set; }
}
