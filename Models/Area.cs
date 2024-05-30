using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_area")]
public class Area
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("area_index")]
    public int AreaIndex { get; set; }

    [Column("reien_index")]
    public int ReienIndex { get; set; }

    [Column("area_code")]
    [MaxLength(100)]
    public string AreaCode { get; set; }

    [Column("area_name")]
    [MaxLength(100)]
    public string AreaName { get; set; }

    [Column("create_date")]
    public DateTime? CreateDate { get; set; }
    [Column("create_user")]
    public int? CreateUser { get; set; }
    [Column("update_date")]
    public DateTime? UpdateDate { get; set; }
    [Column("update_user")]
    public int? UpdateUser { get; set; }
    [Required]
    [Column("delete_flag")]
    public int DeleteFlag { get; set; }
    [Required]
    [ForeignKey("ReienIndex")]
    public virtual Reien Reien { get; set; } = new Reien();


}

