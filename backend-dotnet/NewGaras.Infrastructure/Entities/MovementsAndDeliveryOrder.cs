using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("MovementsAndDeliveryOrder")]
public partial class MovementsAndDeliveryOrder
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long? ProjectId { get; set; }

    [Column("InstallationID")]
    public long? InstallationId { get; set; }

    [Column("CarID")]
    public long CarId { get; set; }

    [Column("DriverID")]
    public long DriverId { get; set; }

    public bool DirectStatus { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    public int? DeliveryNumber { get; set; }

    [ForeignKey("CarId")]
    [InverseProperty("MovementsAndDeliveryOrders")]
    public virtual Car Car { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("MovementsAndDeliveryOrderCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DriverId")]
    [InverseProperty("MovementsAndDeliveryOrders")]
    public virtual Driver Driver { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("MovementsAndDeliveryOrderModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("MovementAndDeliveryOrder")]
    public virtual ICollection<MovementReport> MovementReports { get; set; } = new List<MovementReport>();

    [InverseProperty("MovementAndDeliveryOrder")]
    public virtual ICollection<RequieredCost> RequieredCosts { get; set; } = new List<RequieredCost>();
}
