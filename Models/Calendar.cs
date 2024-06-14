using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_calender")]
public class Calendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("calender_index")]

        public int CalenderIndex { get; set; }

        [Column("reien_index")]
        public int ReienIndex { get; set; }

        [Column("close_day")]
        public DateOnly RegularHoliday { get; set; }

        [ForeignKey("ReienIndex")]
        public virtual Reien Reien { get; set; } = new Reien();
}

