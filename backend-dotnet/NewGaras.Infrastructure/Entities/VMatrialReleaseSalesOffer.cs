using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VMatrialReleaseSalesOffer
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [StringLength(50)]
    public string ContactPersonEmail { get; set; }

    [StringLength(20)]
    public string ContactPersonMobile { get; set; }

    [StringLength(500)]
    public string ContactPersonName { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }
}
