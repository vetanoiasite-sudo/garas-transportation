using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ContractReportTo")]
public partial class ContractReportTo
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ContractID")]
    public long ContractId { get; set; }

    [Column("ReportToID")]
    public long ReportToId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public long ModifiedBy { get; set; }

    [ForeignKey("ContractId")]
    [InverseProperty("ContractReportTos")]
    public virtual ContractDetail Contract { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ContractReportToCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ContractReportToModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ReportToId")]
    [InverseProperty("ContractReportToReportTos")]
    public virtual User ReportTo { get; set; }
}
