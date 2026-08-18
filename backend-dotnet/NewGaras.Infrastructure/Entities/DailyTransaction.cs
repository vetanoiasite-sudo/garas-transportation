using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyTransaction")]
public partial class DailyTransaction
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public bool Status { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column("FromAccountID1")]
    public long FromAccountId1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal FromAmount1 { get; set; }

    [Column("FromCurrencyID1")]
    public int FromCurrencyId1 { get; set; }

    [Column("FromMethodID1")]
    public long? FromMethodId1 { get; set; }

    [Column("FromDTMainType1")]
    [StringLength(50)]
    public string FromDtmainType1 { get; set; }

    [Column("FromExpOrIncTypeID1")]
    public long? FromExpOrIncTypeId1 { get; set; }

    [StringLength(250)]
    public string FromExpOrIncTypeName1 { get; set; }

    [Column("FromExtraIDOfType1")]
    public long? FromExtraIdofType1 { get; set; }

    [Column("FromAccountID2")]
    public long? FromAccountId2 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? FromAmount2 { get; set; }

    [Column("FromCurrencyID2")]
    public int? FromCurrencyId2 { get; set; }

    [Column("FromMethodID2")]
    public long? FromMethodId2 { get; set; }

    [Column("FromDTMainType2")]
    [StringLength(50)]
    public string FromDtmainType2 { get; set; }

    [Column("FromExpOrIncTypeID2")]
    public long? FromExpOrIncTypeId2 { get; set; }

    [StringLength(250)]
    public string FromExpOrIncTypeName2 { get; set; }

    [Column("FromExtraIDOfType2")]
    public long? FromExtraIdofType2 { get; set; }

    [Column("FromAccountID3")]
    public long? FromAccountId3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? FromAmount3 { get; set; }

    [Column("FromCurrencyID3")]
    public int? FromCurrencyId3 { get; set; }

    [Column("FromMethodID3")]
    public long? FromMethodId3 { get; set; }

    [Column("FromDTMainType3")]
    [StringLength(50)]
    public string FromDtmainType3 { get; set; }

    [Column("FromExpOrIncTypeID3")]
    public long? FromExpOrIncTypeId3 { get; set; }

    [StringLength(250)]
    public string FromExpOrIncTypeName3 { get; set; }

    [Column("FromExtraIDOfType3")]
    public long? FromExtraIdofType3 { get; set; }

    [Column("ToAccountID1")]
    public long ToAccountId1 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal ToAmount1 { get; set; }

    [Column("ToCurrencyID1")]
    public int ToCurrencyId1 { get; set; }

    [Column("ToMethodID1")]
    public long? ToMethodId1 { get; set; }

    [Column("ToDTMainType1")]
    [StringLength(50)]
    public string ToDtmainType1 { get; set; }

    [Column("ToExpOrIncTypeID1")]
    public long? ToExpOrIncTypeId1 { get; set; }

    [StringLength(250)]
    public string ToExpOrIncTypeName1 { get; set; }

    [Column("ToExtraIDOfType1")]
    public long? ToExtraIdofType1 { get; set; }

    [Column("ToAccountID2")]
    public long? ToAccountId2 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ToAmount2 { get; set; }

    [Column("ToCurrencyID2")]
    public int? ToCurrencyId2 { get; set; }

    [Column("ToMethodID2")]
    public long? ToMethodId2 { get; set; }

    [Column("ToDTMainType2")]
    [StringLength(50)]
    public string ToDtmainType2 { get; set; }

    [Column("ToExpOrIncTypeID2")]
    public long? ToExpOrIncTypeId2 { get; set; }

    [StringLength(250)]
    public string ToExpOrIncTypeName2 { get; set; }

    [Column("ToExtraIDOfType2")]
    public long? ToExtraIdofType2 { get; set; }

    [Column("ToAccountID3")]
    public long? ToAccountId3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal? ToAmount3 { get; set; }

    [Column("ToCurrencyID3")]
    public int? ToCurrencyId3 { get; set; }

    [Column("ToMethodID3")]
    public long? ToMethodId3 { get; set; }

    [Column("ToDTMainType3")]
    [StringLength(50)]
    public string ToDtmainType3 { get; set; }

    [Column("ToExpOrIncTypeID3")]
    public long? ToExpOrIncTypeId3 { get; set; }

    [StringLength(250)]
    public string ToExpOrIncTypeName3 { get; set; }

    [Column("ToExtraIDOfType3")]
    public long? ToExtraIdofType3 { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DailyTransactionCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DailyTransactionModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
