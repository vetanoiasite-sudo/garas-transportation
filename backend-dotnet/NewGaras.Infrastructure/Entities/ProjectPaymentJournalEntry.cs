using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectPaymentJournalEntry")]
public partial class ProjectPaymentJournalEntry
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectPaymentTermID")]
    public long ProjectPaymentTermId { get; set; }

    [Column("DailyJournalEntryID")]
    public long DailyJournalEntryId { get; set; }

    [ForeignKey("DailyJournalEntryId")]
    [InverseProperty("ProjectPaymentJournalEntries")]
    public virtual DailyJournalEntry DailyJournalEntry { get; set; }
}
