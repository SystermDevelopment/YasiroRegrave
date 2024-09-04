using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YasiroRegrave.Model;

namespace YasiroRegrave.Model
{
    [Table("t_cemetery")]
    public class CemeteryInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cemetery_info_index")]
        public int CemeteryInfoIndex { get; set; }
        [Column("cemetery_index")]
        public int CemeteryIndex { get; set; }
        [Column("area_value")]
        public string? AreaValue { get; set; }
        [Column("release_status")]
        public int? ReleaseStatus { get; set; }
        [Column("unrelated_status")]
        public int? UnrelatedStatus { get; set; } = 0;
        [Column("section_status")]
        public int? SectionStatus { get; set; }
        [Column("section_type")]
        public string? SectionType { get; set; }
        [Column("usage_fee")]
        public string? UsageFee { get; set; }
        [Column("management_fee")]
        public string? ManagementFee { get; set; }
        [Column("stone_fee")]
        public string? StoneFee { get; set; }
        [Column("set_price")]
        public string? SetPrice { get; set; }
        [Column("change_status_date")]
        public DateTime? ChangeStatusDate { get; set; }
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
        [Column("create_user")]
        public int? CreateUser { get; set; }
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
        [Column("update_user")]
        public int? UpdateUser { get; set; }
        [Column("image1_fname")]
        [MaxLength(50)]
        public string? Image1Fname { get; set; }
        [Column("image2_fname")]
        [MaxLength(50)]
        public string? Image2Fname { get; set; }
        [Required]
        [Column("delete_flag")]
        public int DeleteFlag { get; set; }
        [Required]
        [ForeignKey("CemeteryIndex")]
        public virtual Cemetery Cemetery { get; set; }=new Cemetery();
    }
}