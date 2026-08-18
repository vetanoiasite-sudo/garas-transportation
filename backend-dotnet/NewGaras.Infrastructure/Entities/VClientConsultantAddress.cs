using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VClientConsultantAddress
{
    [Column("ID")]
    public long Id { get; set; }

    [Column("ConsultantID")]
    public long ConsultantId { get; set; }

    [Column("CountryID")]
    public int CountryId { get; set; }

    [StringLength(500)]
    public string Country { get; set; }

    [Column("GovernorateID")]
    public int GovernorateId { get; set; }

    [StringLength(500)]
    public string Governorate { get; set; }

    [Required]
    public string Address { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [StringLength(10)]
    public string BuildingNumber { get; set; }

    [StringLength(10)]
    public string Floor { get; set; }

    public string Description { get; set; }
}
