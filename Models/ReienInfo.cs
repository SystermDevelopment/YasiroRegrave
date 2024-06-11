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
    [ForeignKey("Index")]
    public virtual User Users { get; set; } = new User();
    [Required]
    [ForeignKey("Index")]
    public virtual Reien Reiens { get; set; } = new Reien();
}
