using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ClientExtraInfo")]
public partial class ClientExtraInfo
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public long IdentityNumber { get; set; }

    [Column("NationalityID")]
    public long NationalityId { get; set; }

    [Required]
    [StringLength(50)]
    public string Gender { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateOfBirth { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public int? MaritalStatusId { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ClientExtraInfos")]
    public virtual Client Client { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ClientExtraInfoCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("MaritalStatusId")]
    [InverseProperty("ClientExtraInfos")]
    public virtual MaritalStatus MaritalStatus { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ClientExtraInfoModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("NationalityId")]
    [InverseProperty("ClientExtraInfos")]
    public virtual Nationality Nationality { get; set; }
}
