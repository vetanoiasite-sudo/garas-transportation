using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Keyless]
public partial class VSupplierSpeciality
{
    [Column("SupplierSpecialityID")]
    public long SupplierSpecialityId { get; set; }

    [Column("ID")]
    public long Id { get; set; }

    [Column("SpecialityID")]
    public int SpecialityId { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long? ModifiedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Modified { get; set; }

    public bool Active { get; set; }

    [StringLength(500)]
    public string Speciality { get; set; }

    [StringLength(500)]
    public string Name { get; set; }

    public bool? HasLogo { get; set; }
}
