using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("t_reien")]

public class ReienInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("t_reien_index")]
    public int ReienTableIndex { get; set; }

    [Required]
    [Column("user_index")]
    public int UserIndex { get; set; }

    [ForeignKey("UserIndex")]
    public virtual User Users { get; set; } = new User();

    [Required]
    [Column("reien_index")]
    public int ReienIndex { get; set; }

    [ForeignKey("ReienIndex")]
    public virtual Reien Reiens { get; set; } = new Reien();
}
