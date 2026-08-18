using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("TaskUnitRateService")]
public partial class TaskUnitRateService
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string ServiceName { get; set; }

    [Column("UOMID")]
    public int Uomid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Rate { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18, 4)")]
    public decimal Total { get; set; }

    [Column("TaskID")]
    public long TaskId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TaskUnitRateServiceCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("TaskUnitRateServiceModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("TaskId")]
    [InverseProperty("TaskUnitRateServices")]
    public virtual Task Task { get; set; }

    [ForeignKey("Uomid")]
    [InverseProperty("TaskUnitRateServices")]
    public virtual InventoryUom Uom { get; set; }
}
