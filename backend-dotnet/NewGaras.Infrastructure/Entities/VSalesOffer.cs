using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSalesOffer
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesPersonID")]
    public long SalesPersonId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? FinalOfferPrice { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [Column("InventoryItemCategoryID")]
    public int? InventoryItemCategoryId { get; set; }

    [Column("BranchID")]
    public int BranchId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [StringLength(500)]
    public string SalesPersonPhoto { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [Required]
    [StringLength(1000)]
    public string CategoryName { get; set; }

    [StringLength(101)]
    public string SalesPersonFullName { get; set; }
}
