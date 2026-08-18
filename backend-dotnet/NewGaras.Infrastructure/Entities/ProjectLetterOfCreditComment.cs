using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NewGaras.Infrastructure.Entities;

[Table("ProjectLetterOfCreditComment")]
public partial class ProjectLetterOfCreditComment
{
    [Key]
    [Column("ID")]
    public long Id { get; set; }

    [Column("ProjectLetterOfCreditID")]
    public long ProjectLetterOfCreditId { get; set; }

    [Required]
    [StringLength(250)]
    public string Comment { get; set; }

    public long CreatedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreationDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ModificationDate { get; set; }

    public long ModifiedBy { get; set; }

    public bool Active { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("ProjectLetterOfCreditCommentCreatedByNavigations")]
    public virtual User CreatedByNavigation { get; set; }

    [ForeignKey("ModifiedBy")]
    [InverseProperty("ProjectLetterOfCreditCommentModifiedByNavigations")]
    public virtual User ModifiedByNavigation { get; set; }

    [ForeignKey("ProjectLetterOfCreditId")]
    [InverseProperty("ProjectLetterOfCreditComments")]
    public virtual ProjectLetterOfCredit ProjectLetterOfCredit { get; set; }
}
