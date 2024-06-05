using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("t_contact")]
public class Contact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("contact_link_index")]

    public int CntactLinkIndex { get; set; }

    [Column("contact_index")]
    public int ContactIndex { get; set; }

    [Column("reserve_index")]
    public int ReserveIndex { get; set; }
    [ForeignKey("ContactIndex")]
    public virtual Contact Contacts { get; set; } = new Contact();
    [Required]
    [ForeignKey("ReserveIndex")]
    public virtual ReserveInfo Reserve { get; set; } = new ReserveInfo();
}
