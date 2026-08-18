using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TransportationLineIncreaseRequest")]
public partial class TransportationLineIncreaseRequest
{
    [Key]
    public int Id { get; set; }

    [Column("isPercent")]
    public bool IsPercent { get; set; }

    [Column("approximateToFiveFlag")]
    public bool ApproximateToFiveFlag { get; set; }

    [Column("increaseCost", TypeName = "decimal(10, 4)")]
    public decimal IncreaseCost { get; set; }

    [Column("approve")]
    public bool? Approve { get; set; }

    [Column("approvedBy")]
    public long? ApprovedBy { get; set; }

    [Column("approvedDate", TypeName = "datetime")]
    public DateTime? ApprovedDate { get; set; }

    [Column("forAllLines")]
    public bool ForAllLines { get; set; }

    [Column("creationDate", TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column("creationBy")]
    public long CreationBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [ForeignKey("ApprovedBy")]
    [InverseProperty("TransportationLineIncreaseRequestApprovedByNavigations")]
    public virtual User ApprovedByNavigation { get; set; }

    [ForeignKey("CreationBy")]
    [InverseProperty("TransportationLineIncreaseRequestCreationByNavigations")]
    public virtual User CreationByNavigation { get; set; }

    [InverseProperty("TransportationLineIncreaseRequest")]
    public virtual ICollection<TransportationLineIncreaseRequestLine> TransportationLineIncreaseRequestLines { get; set; } = new List<TransportationLineIncreaseRequestLine>();
}
