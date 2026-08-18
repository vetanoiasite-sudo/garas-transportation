using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AdvanciedType")]
public partial class AdvanciedType
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string AdvanciedTypeName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [Column("AccountCategoryID")]
    public long? AccountCategoryId { get; set; }

    [InverseProperty("AdvanciedType")]
    public virtual ICollection<AdvanciedSettingAccount> AdvanciedSettingAccounts { get; set; } = new List<AdvanciedSettingAccount>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("AdvanciedTypeCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AdvanciedTypeModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
