using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("DailyTranactionBeneficiaryToUser")]
public partial class DailyTranactionBeneficiaryToUser
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("DailyTransactionID")]
    public long DailyTransactionId { get; set; }

    [Column("BeneficiaryTypeID")]
    public long BeneficiaryTypeId { get; set; }

    [Required]
    [StringLength(250)]
    public string BeneficiaryTypeName { get; set; }

    [Column("BeneficiaryUserID")]
    public long BeneficiaryUserId { get; set; }

    [StringLength(250)]
    public string BeneficiaryUserName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool Active { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    [ForeignKey("BeneficiaryTypeId")]
    [InverseProperty("DailyTranactionBeneficiaryToUsers")]
    public virtual DailyTranactionBeneficiaryToType BeneficiaryType { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("DailyTranactionBeneficiaryToUserCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("DailyTranactionBeneficiaryToUserModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
