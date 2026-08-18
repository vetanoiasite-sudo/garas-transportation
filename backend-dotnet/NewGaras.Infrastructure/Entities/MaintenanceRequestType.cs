using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MaintenanceRequestType")]
public partial class MaintenanceRequestType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(450)]
    public string Name { get; set; }

    [Required]
    [StringLength(450)]
    public string ArName { get; set; }

    public bool Active { get; set; }

    [InverseProperty("MaintenanceRequestType")]
    public virtual ICollection<MangmentOfMaintenanceRequest> MangmentOfMaintenanceRequests { get; set; } = new List<MangmentOfMaintenanceRequest>();
}
