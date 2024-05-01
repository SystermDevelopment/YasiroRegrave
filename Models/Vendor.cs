using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
[Table("m_vender")]
public class Vender
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("vender_index")]
    public int VenderIndex { get; set; }
    [Required]
    [Column("vender_name")]
    [MaxLength(100)]
    public string VenderName { get; set; }
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
}