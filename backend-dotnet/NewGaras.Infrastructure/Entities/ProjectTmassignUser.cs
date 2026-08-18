using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectTMAssignUser")]
public partial class ProjectTmassignUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ProjectTMID")]
    public long ProjectTmid { get; set; }

    public bool IsDepartment { get; set; }

    public bool IsTeam { get; set; }

    [Column("MemberID")]
    public long MemberId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectTmassignUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectTmassignUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectTmid")]
    [InverseProperty("ProjectTmassignUsers")]
    public virtual ProjectTm ProjectTm { get; set; }
}
