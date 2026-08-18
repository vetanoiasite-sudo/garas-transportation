using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("POApprovalStatus")]
public partial class PoapprovalStatus
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Column("POApprovalSettingID")]
    public int PoapprovalSettingId { get; set; }

    [Column("ApprovalUserID")]
    public long ApprovalUserId { get; set; }

    public bool IsApproved { get; set; }

    public string Comment { get; set; }

    [Required]
    [StringLength(250)]
    public string CreationBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [ForeignKey("ApprovalUserId")]
    [InverseProperty("PoapprovalStatuses")]
    public virtual User ApprovalUser { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PoapprovalStatuses")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("PoapprovalSettingId")]
    [InverseProperty("PoapprovalStatuses")]
    public virtual PoapprovalSetting PoapprovalSetting { get; set; }
}
