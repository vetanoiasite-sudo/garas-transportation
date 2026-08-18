using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ExchangeRate")]
public partial class ExchangeRate
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("FromCurrencyID")]
    public int FromCurrencyId { get; set; }

    [Column("ToCurrencyID")]
    public int ToCurrencyId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Rate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ExchangeRateCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("FromCurrencyId")]
    [InverseProperty("ExchangeRateFromCurrencies")]
    public virtual Currency FromCurrency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ExchangeRateModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ToCurrencyId")]
    [InverseProperty("ExchangeRateToCurrencies")]
    public virtual Currency ToCurrency { get; set; }
}
