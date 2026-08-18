using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VPricingFullDatum
{
    [Column("PricingID")]
    public long PricingId { get; set; }

    [Required]
    [StringLength(100)]
    public string Status { get; set; }

    public bool Active { get; set; }

    public bool SalesHeadApprove { get; set; }

    public bool PricingHeadApprove { get; set; }

    [Column("PricingPersonID")]
    public long? PricingPersonId { get; set; }

    [Required]
    [StringLength(250)]
    public string PricingType { get; set; }

    public bool Completed { get; set; }

    [Column("RefID")]
    public long? RefId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [Column("SalesPersonID")]
    public long? SalesPersonId { get; set; }

    public bool? SalesActive { get; set; }

    public int? VersionNumber { get; set; }

    [StringLength(500)]
    public string ProductType { get; set; }

    [StringLength(500)]
    public string ProjectName { get; set; }

    [Column("ClientID")]
    public long? ClientId { get; set; }

    [StringLength(500)]
    public string Name { get; set; }

    [StringLength(50)]
    public string FirstName { get; set; }

    public bool? UserActive { get; set; }

    [StringLength(50)]
    public string LastName { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    public byte[] Photo { get; set; }

    [Column("PricingBranshID")]
    public int PricingBranshId { get; set; }
}
