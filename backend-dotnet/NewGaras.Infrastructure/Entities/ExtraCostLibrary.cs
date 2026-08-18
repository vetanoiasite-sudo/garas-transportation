using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ExtraCostLibrary")]
public partial class ExtraCostLibrary
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Name { get; set; }

    [Required]
    [StringLength(250)]
    public string Type { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DefaultPrice { get; set; }

    [Column("DefaultCurrencyID")]
    public int DefaultCurrencyId { get; set; }

    public bool Active { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ExtraCostLibraryCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("DefaultCurrencyId")]
    [InverseProperty("ExtraCostLibraries")]
    public virtual Currency DefaultCurrency { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ExtraCostLibraryModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
