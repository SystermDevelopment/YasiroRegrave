using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("reserve_info")]
public class Reserve_Info
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("reserve_index")]
    public int Reserve_Index { get; set; }
    [Required]
    [Column("cemetery_info_index")]
    public int Cemetery_Info_Index { get; set; }
    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string Last_Name { get; set; }
    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string First_Name { get; set; }
    [Required]
    [Column("last_name_yomi")]
    [MaxLength(100)]
    public string Last_Name_yomi { get; set; }
    [Required]
    [Column("first_name_yomi")]
    [MaxLength(100)]
    public string First_Name_yomi { get; set; }
    [Required]
    [Column("zip_code")]
    [MaxLength(7)]
    public string Zip_Code { get; set; }
    [Required]
    [Column("adress")]
    [MaxLength(255)]
    public string Adress { get; set; }
    [Required]
    [Column("telephone_number")]
    [MaxLength(11)]
    public string Telephone_Number { get; set; }
    [Required]
    [Column("e_mail")]
    [MaxLength(100)]
    public string E_Mail { get; set; }
    [Required]
    [Column("question")]
    [MaxLength(500)]
    public string Question { get; set; }
    [Required]
    [Column("vender_index")]
    public int VenderIndex { get; set; }
}