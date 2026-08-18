using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPoapprovalStatus
{
    [Column("ID")]
    public long Id { get; set; }

    public bool Mandatory { get; set; }

    [Required]
    [StringLength(400)]
    public string Name { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Column("POApprovalSettingID")]
    public int PoapprovalSettingId { get; set; }

    [Column("ApprovalUserID")]
    public long ApprovalUserId { get; set; }

    public bool IsApproved { get; set; }

    public string Comment { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }
}
