using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VTaskMangerProject
{
    [Required]
    [StringLength(250)]
    public string AttachmentPath { get; set; }

    [Required]
    [StringLength(50)]
    public string FileName { get; set; }

    [Column("SalesOfferID")]
    public long SalesOfferId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("ProjectManagerID")]
    public long? ProjectManagerId { get; set; }

    public bool? Billable { get; set; }

    public bool? TimeTracking { get; set; }

    [Column("PriortyID")]
    public int? PriortyId { get; set; }

    [Column(TypeName = "decimal(18, 3)")]
    public decimal? Budget { get; set; }

    [Column("CurrencyID")]
    public int? CurrencyId { get; set; }

    public bool? FromTaskManger { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [StringLength(50)]
    public string OfferType { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(1000)]
    public string ProjectLocation { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; }

    [Column("projectDescription")]
    public string ProjectDescription { get; set; }
}
