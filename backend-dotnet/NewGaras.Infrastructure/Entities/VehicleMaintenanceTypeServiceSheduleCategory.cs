using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("VehicleMaintenanceTypeServiceSheduleCategory")]
public partial class VehicleMaintenanceTypeServiceSheduleCategory
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long VehicleServiceScheduleCategoryId { get; set; }

    public long VehicleMaintenanceTypeId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [ForeignKey("VehicleMaintenanceTypeId")]
    [InverseProperty("VehicleMaintenanceTypeServiceSheduleCategories")]
    public virtual VehicleMaintenanceType VehicleMaintenanceType { get; set; }

    [ForeignKey("VehicleServiceScheduleCategoryId")]
    [InverseProperty("VehicleMaintenanceTypeServiceSheduleCategories")]
    public virtual VehicleServiceScheduleCategory VehicleServiceScheduleCategory { get; set; }
}
