using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("PuchasePOShipment")]
public partial class PuchasePoshipment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("POID")]
    public long Poid { get; set; }

    [Column("ShipmentShippingMethodDetailsID")]
    public long ShipmentShippingMethodDetailsId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ExpectedReceivingDate { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("PuchasePoshipmentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("PuchasePoshipmentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("Poid")]
    [InverseProperty("PuchasePoshipments")]
    public virtual PurchasePo Po { get; set; }

    [ForeignKey("ShipmentShippingMethodDetailsId")]
    [InverseProperty("PuchasePoshipments")]
    public virtual PurchasePoshipmentShippingMethodDetail ShipmentShippingMethodDetails { get; set; }
}
