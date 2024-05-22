using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("reserve_info")]
public class Reserve_Info
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("reserve_index")]
    public int ReserveIndex { get; set; }
    [Required]
    [Column("cemetery_info_index")]
    public int CemeteryInfoIndex { get; set; }
    [Required]
    [Column("last_name")]
    [MaxLength(100)]
    public string LastName { get; set; }
    [Required]
    [Column("first_name")]
    [MaxLength(100)]
    public string FirstName { get; set; }
    [Required]
    [Column("last_name_yomi")]
    [MaxLength(100)]
    public string LastNameYomi { get; set; }
    [Required]
    [Column("first_name_yomi")]
    [MaxLength(100)]
    public string FirstNameYomi { get; set; }
    [Required]
    [Column("zip_code")]
    [MaxLength(7)]
    public string ZipCode { get; set; }
    [Required]
    [Column("adress")]
    [MaxLength(255)]
    public string Adress { get; set; }
    [Required]
    [Column("telephone_number")]
    [MaxLength(11)]
    public string TelephoneNumber { get; set; }
    [Required]
    [Column("e_mail")]
    [MaxLength(100)]
    public string EMail { get; set; }
    [Required]
    [Column("question")]
    [MaxLength(500)]
    public string Question { get; set; }
    [Required]
    [Column("vender_index")]
    public int VenderIndex { get; set; }
}