using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class PaymentTerm
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string PaymentTermName { get; set; }

    [InverseProperty("PaymentTerm")]
    public virtual ICollection<ProjectPaymentTerm> ProjectPaymentTerms { get; set; } = new List<ProjectPaymentTerm>();
}
