using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("EInvoiceSetting")]
public partial class EinvoiceSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public bool IsProduction { get; set; }

    public string ClientIdProduction { get; set; }

    public string Clientsecret1Production { get; set; }

    public string Clientsecret2Production { get; set; }

    public string ClientIdTest { get; set; }

    public string Clientsecret1Test { get; set; }

    public string Clientsecret2Test { get; set; }

    [StringLength(250)]
    public string IssuerType { get; set; }

    [Required]
    [StringLength(250)]
    public string RegistrationNumber { get; set; }

    public bool Active { get; set; }

    [Required]
    [StringLength(250)]
    public string CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Required]
    [StringLength(250)]
    public string ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModifiedDate { get; set; }

    [StringLength(50)]
    public string ActivityCode1 { get; set; }

    [StringLength(50)]
    public string ActivityCode2 { get; set; }

    [StringLength(50)]
    public string ActivityCode3 { get; set; }

    public bool IsSignature { get; set; }

    [Required]
    [StringLength(50)]
    public string ItemCodeType { get; set; }

    [Required]
    [Column("ItemTypeEINVOICE")]
    [StringLength(50)]
    public string ItemTypeEinvoice { get; set; }

    [StringLength(50)]
    public string IssureName { get; set; }

    [StringLength(50)]
    public string CompanyName { get; set; }

    [StringLength(50)]
    public string PinCode { get; set; }

    [StringLength(50)]
    public string Path { get; set; }
}
