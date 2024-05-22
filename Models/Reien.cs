using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_reien")]
public class Reien
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("reien_index")]
    public int Index { get; set; }
    [Required]
    [Column("reien_code")]
    [MaxLength(10)]
    public string Code { get; set; }
    [Required]
    [Column("reien_name")]
    [MaxLength(100)]
    public string Name { get; set; }
    [Column("mail_address")]
    [MaxLength(500)]
    public string MailAddress { get; set; }
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