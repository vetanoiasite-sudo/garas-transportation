using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VDailyJournalEntry
{
    [Column("ID")]
    public long Id { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [StringLength(250)]
    public string Serial { get; set; }

    public bool Closed { get; set; }

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

    [Required]
    [StringLength(50)]
    public string CreatorFirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string CreatorLastName { get; set; }
}
