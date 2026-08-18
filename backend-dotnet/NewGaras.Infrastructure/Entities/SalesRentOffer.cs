using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("SalesRentOffer")]
public partial class SalesRentOffer
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("RentSalesOfferID")]
    public long RentSalesOfferId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RentFromDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RentToDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModificationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("SalesRentOfferCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("SalesRentOfferModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
