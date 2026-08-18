using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ContractSetting
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public bool FreeWorkingHours { get; set; }

    public bool AllowBreakDeduction { get; set; }

    public bool AllowLocationTracking { get; set; }

    public double? Diameter { get; set; }

    public bool AllowComissions { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? ComissionRate { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal? ComissionPercentage { get; set; }

    public bool AllowOvertime { get; set; }

    public bool? AutomaticPerWorkingHours { get; set; }

    public bool? AllowedWithApproval { get; set; }

    [Column("ContractID")]
    public long ContractId { get; set; }

    [ForeignKey("ContractId")]
    [InverseProperty("ContractSettings")]
    public virtual ContractDetail Contract { get; set; } = null!;
}
