using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ClientConsultant")]
public partial class ClientConsultant
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ClientID")]
    public long ClientId { get; set; }

    [Required]
    [StringLength(500)]
    public string ConsultantName { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [StringLength(1000)]
    public string Company { get; set; }

    [StringLength(250)]
    public string ConsultantFor { get; set; }

    [InverseProperty("Consultant")]
    public virtual ICollection<ClientConsultantEmail> ClientConsultantEmails { get; set; } = new List<ClientConsultantEmail>();

    [InverseProperty("Consultant")]
    public virtual ICollection<ClientConsultantFax> ClientConsultantFaxes { get; set; } = new List<ClientConsultantFax>();

    [InverseProperty("Consultant")]
    public virtual ICollection<ClientConsultantMobile> ClientConsultantMobiles { get; set; } = new List<ClientConsultantMobile>();

    [InverseProperty("Consultant")]
    public virtual ICollection<ClientConsultantPhone> ClientConsultantPhones { get; set; } = new List<ClientConsultantPhone>();

    [InverseProperty("Consultant")]
    public virtual ICollection<ClientConsultantSpecialilty> ClientConsultantSpecialilties { get; set; } = new List<ClientConsultantSpecialilty>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("ClientConsultantCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ClientConsultantModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }
}
