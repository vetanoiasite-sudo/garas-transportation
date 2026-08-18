using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class StpTaxTypeV
{
    [Column("subTaxTypeID")]
    public long SubTaxTypeId { get; set; }

    [Column("subTaxTypeName")]
    [StringLength(250)]
    public string SubTaxTypeName { get; set; }

    [StringLength(250)]
    public string SubTaxTypeCode { get; set; }

    [StringLength(250)]
    public string TaxTypeName { get; set; }

    [StringLength(250)]
    public string TaxType { get; set; }

    [Column("isPercentage")]
    public bool? IsPercentage { get; set; }

    public int LastOne { get; set; }
}
