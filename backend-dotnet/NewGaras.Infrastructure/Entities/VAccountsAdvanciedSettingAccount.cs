using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VAccountsAdvanciedSettingAccount
{
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(250)]
    public string AccountName { get; set; }

    public bool Haveitem { get; set; }

    public bool Active { get; set; }

    public bool AdvanciedSettingsStatus { get; set; }

    [Column("AdvanciedTypeID")]
    public long? AdvanciedTypeId { get; set; }

    [StringLength(250)]
    public string AdvanciedTypeName { get; set; }

    [Column("KeeperID")]
    public long? KeeperId { get; set; }

    [StringLength(250)]
    public string KeeperName { get; set; }

    [StringLength(250)]
    public string AccountNumber { get; set; }
}
