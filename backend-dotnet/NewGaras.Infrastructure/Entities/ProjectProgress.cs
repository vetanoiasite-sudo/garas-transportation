using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectProgress")]
public partial class ProjectProgress
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectID")]
    public long ProjectId { get; set; }

    [Column("ProgressTypeID")]
    public int ProgressTypeId { get; set; }

    [Column("DeliveryTypeID")]
    public int DeliveryTypeId { get; set; }

    [Column("ProgressStatusID")]
    public int ProgressStatusId { get; set; }

    [Column(TypeName = "decimal(8, 4)")]
    public decimal RelatedCollectedPercent { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    public string AttachmentPath { get; set; }

    public string Comment { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectProgressCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DeliveryTypeId")]
    [InverseProperty("ProjectProgresses")]
    public virtual DeliveryType DeliveryType { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectProgressModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProgressStatusId")]
    [InverseProperty("ProjectProgresses")]
    public virtual ProgressStatus ProgressStatus { get; set; }

    [ForeignKey("ProgressTypeId")]
    [InverseProperty("ProjectProgresses")]
    public virtual ProgressType ProgressType { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("ProjectProgresses")]
    public virtual Project Project { get; set; }

    [InverseProperty("ProjectProgress")]
    public virtual ICollection<ProjectProgressUser> ProjectProgressUsers { get; set; } = new List<ProjectProgressUser>();
}
