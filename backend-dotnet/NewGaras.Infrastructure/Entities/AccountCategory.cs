using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AccountCategory")]
public partial class AccountCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string AccountCategoryName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [InverseProperty("AccountCategory")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("AccountCategoryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AccountCategoryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
