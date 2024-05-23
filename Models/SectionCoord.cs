using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_section_coord")]
public class SectionCoord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("coord_index")]

        public int CoordIndex { get; set; }

        [Column("section_index")]
        public int SectionIndex { get; set; }

        [Column("x")]
        public int X { get; set; }

        [Column("y")]
        public int Y { get; set; }
        [ForeignKey("VenderIndex")]
        public virtual SectionCoord SectionCoords { get; set; } = new SectionCoord();

}

