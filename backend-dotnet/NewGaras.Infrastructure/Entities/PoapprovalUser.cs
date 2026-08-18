using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("POApprovalUser")]
public partial class PoapprovalUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("POApprovalSettingID")]
    public int PoapprovalSettingId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(250)]
    public string CreatedBy { get; set; }

    [ForeignKey("PoapprovalSettingId")]
    [InverseProperty("PoapprovalUsers")]
    public virtual PoapprovalSetting PoapprovalSetting { get; set; }
}
