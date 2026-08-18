using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class ProjectProgressUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long ProjectProgressId { get; set; }

    public long HrUserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateFrom { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateTo { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal HoursNum { get; set; }

    [Column(TypeName = "decimal(18, 8)")]
    public decimal? Evaluation { get; set; }

    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    public bool Active { get; set; }

    public int? InventoryItemCategoryId { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectProgressUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("ProjectProgressUsers")]
    public virtual HrUser HrUser { get; set; }

    [ForeignKey("InventoryItemCategoryId")]
    [InverseProperty("ProjectProgressUsers")]
    public virtual InventoryItemCategory InventoryItemCategory { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectProgressUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectProgressId")]
    [InverseProperty("ProjectProgressUsers")]
    public virtual ProjectProgress ProjectProgress { get; set; }
}
