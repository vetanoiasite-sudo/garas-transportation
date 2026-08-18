using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("AdvanciedSettingAccount")]
public partial class AdvanciedSettingAccount
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("AccountID")]
    public long AccountId { get; set; }

    [Column("AdvanciedTypeID")]
    public long AdvanciedTypeId { get; set; }

    [StringLength(250)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(500)]
    public string Location { get; set; }

    [Column("KeeperID")]
    public long? KeeperId { get; set; }

    [StringLength(250)]
    public string KeeperName { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("AdvanciedSettingAccounts")]
    public virtual Account Account { get; set; }

    [ForeignKey("AdvanciedTypeId")]
    [InverseProperty("AdvanciedSettingAccounts")]
    public virtual AdvanciedType AdvanciedType { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AdvanciedSettingAccountCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("AdvanciedSettingAccountModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
