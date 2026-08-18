using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
[Table("ProAuto_AccountTreeTEMP")]
public partial class ProAutoAccountTreeTemp
{
    [Column("ID")]
    public byte Id { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountName { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string Type { get; set; }

    [Column("CuID")]
    public byte CuId { get; set; }

    public byte ParentCategory { get; set; }

    public byte DataLevel { get; set; }

    public byte AccountOrder { get; set; }

    [Required]
    [StringLength(50)]
    public string Descriptio { get; set; }

    public byte Active { get; set; }

    public bool HaveItem { get; set; }

    public byte Accumulative { get; set; }

    public byte Credit { get; set; }

    public byte Debit { get; set; }

    [Required]
    [Column("comment")]
    [StringLength(50)]
    public string Comment { get; set; }

    [Column("havetax")]
    public byte Havetax { get; set; }

    [Required]
    [Column("TaxID")]
    [StringLength(50)]
    public string TaxId { get; set; }

    [Required]
    [StringLength(50)]
    public string TaxName { get; set; }

    [Required]
    [StringLength(50)]
    public string Tax { get; set; }

    public DateOnly CreationDate { get; set; }

    public byte Modifiedby { get; set; }

    public DateOnly ModifiedDate { get; set; }

    public byte Createdby { get; set; }

    [Column("AccountCategoryID")]
    public byte AccountCategoryId { get; set; }

    public bool AdvancedSettingstatus { get; set; }

    public byte TransactionStatus { get; set; }
}
