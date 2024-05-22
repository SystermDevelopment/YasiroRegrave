using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_contact")]
public class m_Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("contact_index")]

        public int CntactIndex { get; set; }

        [Column("contact")]
        [MaxLength(100)]

        public int Contact { get; set; }
    }

