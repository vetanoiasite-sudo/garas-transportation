using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

public partial class SalaryAllownce
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SalaryID")]
    public long SalaryId { get; set; }

    [Column("AllowncesTypeID")]
    public int AllowncesTypeId { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Amount { get; set; }

    [Column("percentage", TypeName = "decimal(5, 2)")]
    public decimal Percentage { get; set; }

    public bool? IsArchived { get; set; }

    [ForeignKey("AllowncesTypeId")]
    [InverseProperty("SalaryAllownces")]
    public virtual AllowncesType AllowncesType { get; set; }

    [ForeignKey("SalaryId")]
    [InverseProperty("SalaryAllownces")]
    public virtual Salary Salary { get; set; }
}
