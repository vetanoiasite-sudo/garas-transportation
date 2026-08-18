using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Car
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DriverID")]
    public long DriverId { get; set; }

    [Required]
    [StringLength(500)]
    public string Model { get; set; }

    [Required]
    [StringLength(500)]
    public string CarNumber { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public byte[] Photo { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [InverseProperty("Car")]
    public virtual ICollection<CarsAttachment> CarsAttachments { get; set; } = new List<CarsAttachment>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("CarCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DriverId")]
    [InverseProperty("Cars")]
    public virtual Driver Driver { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("CarModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Car")]
    public virtual ICollection<MovementsAndDeliveryOrder> MovementsAndDeliveryOrders { get; set; } = new List<MovementsAndDeliveryOrder>();
}
