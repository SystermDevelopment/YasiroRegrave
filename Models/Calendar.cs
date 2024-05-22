using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_carender")]
public class Calendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("calender_index")]

        public int CalenderIndex { get; set; }

        [Column("reien_index")]
        public int ReienIndex { get; set; }
    }

