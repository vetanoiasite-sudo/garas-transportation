using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("BankDetail")]
public partial class BankDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string BankName { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountHolder { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountNumber { get; set; }

    [Required]
    [StringLength(50)]
    public string BankBranch { get; set; }

    [StringLength(50)]
    public string ExpiryDate { get; set; }

    [Column("HrUserID")]
    public long? HrUserId { get; set; }

    [ForeignKey("HrUserId")]
    [InverseProperty("BankDetails")]
    public virtual HrUser HrUser { get; set; }
}
