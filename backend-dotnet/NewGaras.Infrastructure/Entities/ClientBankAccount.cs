using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ClientBankAccount")]
public partial class ClientBankAccount
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    public string BankDetails { get; set; }

    [StringLength(500)]
    public string BeneficiaryName { get; set; }

    [Column("IBAN")]
    [StringLength(500)]
    public string Iban { get; set; }

    [StringLength(500)]
    public string SwiftCode { get; set; }

    [StringLength(500)]
    public string Account { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientBankAccounts")]
    public virtual Client Client { get; set; }
}
