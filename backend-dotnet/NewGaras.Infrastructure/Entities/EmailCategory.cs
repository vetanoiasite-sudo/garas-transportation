using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("EmailCategory")]
public partial class EmailCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("EmailID")]
    public long EmailId { get; set; }

    [Column("CategoryTypeID")]
    public long CategoryTypeId { get; set; }

    [Column("TypeID")]
    public long TypeId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    [ForeignKey("CategoryTypeId")]
    [InverseProperty("EmailCategories")]
    public virtual EmailCategoryType CategoryType { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("EmailCategoryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("EmailCategories")]
    public virtual Email Email { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("EmailCategoryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
