using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class Driver
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    public byte[] Photo { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long CreatedBy { get; set; }

    [InverseProperty("Driver")]
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("DriverCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [InverseProperty("Driver")]
    public virtual ICollection<DriversAttachment> DriversAttachments { get; set; } = new List<DriversAttachment>();

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DriverModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [InverseProperty("Driver")]
    public virtual ICollection<MovementsAndDeliveryOrder> MovementsAndDeliveryOrders { get; set; } = new List<MovementsAndDeliveryOrder>();
}
