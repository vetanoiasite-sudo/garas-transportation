using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectFabricationWorkshopStationHistory")]
public partial class ProjectFabricationWorkshopStationHistory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long FabricationOrderId { get; set; }

    public long ProjectWorkshopStationId { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    public bool IsCurrent { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("FabricationOrderId")]
    [InverseProperty("ProjectFabricationWorkshopStationHistories")]
    public virtual ProjectFabrication FabricationOrder { get; set; }

    [ForeignKey("ProjectWorkshopStationId")]
    [InverseProperty("ProjectFabricationWorkshopStationHistories")]
    public virtual ProjectWorkshopStation ProjectWorkshopStation { get; set; }
}
