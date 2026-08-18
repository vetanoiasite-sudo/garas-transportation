using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("UserTeam")]
public partial class UserTeam
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public long? UserId { get; set; }

    [Column("TeamID")]
    public long TeamId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [Column("HrUserID")]
    public long? HrUserId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserTeamCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("UserTeams")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("UserTeamModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TeamId")]
    [InverseProperty("UserTeams")]
    public virtual Team Team { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserTeamUsers")]
    public virtual User User { get; set; }
}
