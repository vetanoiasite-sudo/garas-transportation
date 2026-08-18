using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BankChequeTemplate")]
public partial class BankChequeTemplate
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string BankName { get; set; }

    [Required]
    public string ChequeTemnplatePath { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BankChequeTemplateCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("BankChequeTemplateModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
