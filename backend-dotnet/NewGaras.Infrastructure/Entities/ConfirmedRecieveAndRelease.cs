using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ConfirmedRecieveAndRelease")]
public partial class ConfirmedRecieveAndRelease
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DailyAdjustingEntryID")]
    public long DailyAdjustingEntryId { get; set; }

    [Column("AccountID")]
    public long AccountId { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Amount { get; set; }

    [StringLength(500)]
    public string Comment { get; set; }

    [StringLength(50)]
    public string ChequNumber { get; set; }

    public long? ReceivedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReceivedDate { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public bool RecieveStatus { get; set; }

    public bool ReleaseStatus { get; set; }

    [ForeignKey("AccountId")]
    [InverseProperty("ConfirmedRecieveAndReleases")]
    public virtual Account Account { get; set; }

    [InverseProperty("ConfirmedRecieveAndRelease")]
    public virtual ICollection<ConfirmedRecieveAndReleaseAttachment> ConfirmedRecieveAndReleaseAttachments { get; set; } = new List<ConfirmedRecieveAndReleaseAttachment>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("ConfirmedRecieveAndReleaseCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ConfirmedRecieveAndReleaseModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ReceivedBy")]
    [InverseProperty("ConfirmedRecieveAndReleaseReceivedByNavigations")]
    public virtual User ReceivedByNavigation { get; set; }
}
