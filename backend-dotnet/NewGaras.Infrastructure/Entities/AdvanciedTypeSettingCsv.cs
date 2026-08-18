using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
[Table("AdvanciedTypeSettingCSV")]
public partial class AdvanciedTypeSettingCsv
{
    [Column("AccountID")]
    public long AccountId { get; set; }

    [Column("AdvancedTypeID")]
    public long AdvancedTypeId { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Description { get; set; }

    [Required]
    [StringLength(50)]
    public string Location { get; set; }

    [Required]
    [Column("Keeper_ID")]
    [StringLength(50)]
    public string KeeperId { get; set; }

    [Required]
    [Column("Keeper_Name")]
    [StringLength(50)]
    public string KeeperName { get; set; }

    public int Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long Modifiedby { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModiifedDate { get; set; }

    [Column("created_by")]
    public long? CreatedBy { get; set; }
}
