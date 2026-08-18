using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VProjectReturnSalesOffer
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("ParentSalesOfferID")]
    public long ParentSalesOfferId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ParentFinalOfferPrice { get; set; }

    [StringLength(250)]
    public string ParentOfferSerial { get; set; }

    [StringLength(50)]
    public string ParentOfferType { get; set; }

    public long ParentSalesPersonId { get; set; }

    [StringLength(250)]
    public string ParentOfferStatus { get; set; }

    public long ParentProjectId { get; set; }

    public bool ParentProjectClosed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ParentProjectExtraCost { get; set; }

    [StringLength(250)]
    public string ParentProjectSerial { get; set; }

    public long ReturnSalesOfferId { get; set; }

    [Column(TypeName = "decimal(18, 5)")]
    public decimal? ReturnFinalOfferPrice { get; set; }

    [StringLength(250)]
    public string ReturnOfferSerial { get; set; }

    [StringLength(50)]
    public string ReturnOfferType { get; set; }

    public long ReturnSalesPersonId { get; set; }

    [StringLength(250)]
    public string ReturnOfferStatus { get; set; }

    public long ReturnProjectId { get; set; }

    public bool ReturnProjectClosed { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? ReturnProjectExtraCost { get; set; }

    [StringLength(250)]
    public string ReturnProjectSerial { get; set; }
}
