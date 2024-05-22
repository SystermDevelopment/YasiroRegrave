using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_area_coord")]
public class AreaCoord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("coord_index")]

        public int CoordIndex { get; set; }

        [Column("area_index")]
        public int AreaIndex { get; set; }

        [Column("x")]
        public int X { get; set; }

        [Column("y")]
        public int Y { get; set; }
        [ForeignKey("AreaIndex")]
        public virtual Area Area { get; set; } = new Area();

}

