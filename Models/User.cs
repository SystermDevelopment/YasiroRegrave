using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_user")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("user_index")]
    public int UserIndex { get; set; }
    [Required]
    [Column("user_id")]
    [MaxLength(20)]
    public string UserId { get; set; }
    [Required]
    [Column("user_name")]
    [MaxLength(100)]
    public string UserName { get; set; }
    [Required]
    [Column("vender_index")]
    public int VenderIndex { get; set; }
    [Required]
    [Column("password")]
    public string Password { get; set; }
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
    [ForeignKey("VenderIndex")]
    public virtual Vender Vendor { get; set; } = new Vender();
}