using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ProjectFabricationOrderUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectFabricationID")]
    public long ProjectFabricationId { get; set; }

    [Column("UserID")]
    public long UserId { get; set; }

    public bool OrderCloserUser { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectFabricationOrderUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectFabricationOrderUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectFabricationId")]
    [InverseProperty("ProjectFabricationOrderUsers")]
    public virtual ProjectFabrication ProjectFabrication { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("ProjectFabricationOrderUserUsers")]
    public virtual User User { get; set; }
}
