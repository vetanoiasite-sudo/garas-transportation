using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyAdjustingEntry")]
public partial class DailyAdjustingEntry
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public bool Closed { get; set; }

    [Column("FromAccountAdjustingEntryID1")]
    public long FromAccountAdjustingEntryId1 { get; set; }

    [Column("FromAccountAdjustingEntryID2")]
    public long? FromAccountAdjustingEntryId2 { get; set; }

    [Column("FromAccountAdjustingEntryID3")]
    public long? FromAccountAdjustingEntryId3 { get; set; }

    [Column("ToAccountAdjustingEntryID1")]
    public long ToAccountAdjustingEntryId1 { get; set; }

    [Column("ToAccountAdjustingEntryID2")]
    public long? ToAccountAdjustingEntryId2 { get; set; }

    [Column("ToAccountAdjustingEntryID3")]
    public long? ToAccountAdjustingEntryId3 { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    public string DocumentNumber { get; set; }

    public bool Approval { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime EntryDate { get; set; }
}
