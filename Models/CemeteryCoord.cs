using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_cemetery_coord")]
public class CemeteryCoord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("coord_index")]

    public int CoordIndex { get; set; }

    [Column("cemetery_index")]
    public int CemeteryIndex { get; set; }

    [Column("x")]
    public int X { get; set; }

    [Column("y")]
    public int Y { get; set; }
}
