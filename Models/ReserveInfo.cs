using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("reserve_info")]
public class ReserveInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("reserve_index")]
    public int ReserveIndex { get; set; }
    [Required]
    [Column("cemetery_info_index")]
    public int CemeteryInfoIndex { get; set; }
    [Column("preferred_date1")]
    public DateTime? PreferredDate1 { get; set; }
    [Column("preferred_date2")]
    public DateTime? PreferredDate2 { get; set; }
    [Column("preferred_date3")]
    public DateTime? PreferredDate3 { get; set; }
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
    [Column("address")]
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
    [Column("question")]
    [MaxLength(500)]
    public string? Question { get; set; }
    [Column("area_value")]
    public string? AreaValue { get; set; }
    [Column("usage_fee")]
    public string? UsageFee { get; set; }
    [Column("management_fee")]
    public string? ManagementFee { get; set; }
    [Column("stone_fee")]
    public string? StoneFee { get; set; }
    [Column("set_price")]
    public string? SetPrice { get; set; }
    [Column("vender_index")]
    public int? VenderIndex { get; set; }
    [Required]
    [Column("notification")]
    public int Notification { get; set; }
    [Column("create_date")]
    public DateTime? CreateDate { get; set; }
    [Column("create_user")]
    public int? CreateUser { get; set; }
    [Column("update_date")]
    public DateTime? UpdateDate { get; set; }
    [Column("update_user")]
    public int? UpdateUser { get; set; }
    [ForeignKey("CemeteryInfoIndex")]
    public virtual CemeteryInfo CemeteryInfo { get; set; } = new CemeteryInfo();
    [ForeignKey("VenderIndex")]
    public virtual Vender? Vender { get; set; }
}