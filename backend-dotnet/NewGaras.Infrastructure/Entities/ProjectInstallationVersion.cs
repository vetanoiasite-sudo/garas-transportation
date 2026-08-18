using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectInstallationVersion")]
public partial class ProjectInstallationVersion
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    public int Revision { get; set; }

    [Column("ProjectInstallationID")]
    public long ProjectInstallationId { get; set; }

    [Required]
    [StringLength(500)]
    public string FileName { get; set; }

    [Required]
    [StringLength(5)]
    public string FileExtension { get; set; }

    [Required]
    [StringLength(1000)]
    public string FilePath { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ModifiedDate { get; set; }

    public long? ModifiedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectInstallationVersions")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ProjectInstallationId")]
    [InverseProperty("ProjectInstallationVersions")]
    public virtual ProjectInstallation ProjectInstallation { get; set; }
}
