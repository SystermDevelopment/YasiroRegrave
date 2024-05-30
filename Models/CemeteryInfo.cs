using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using YasiroRegrave.Model;

namespace YasiroRegrave.Models
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
        public float? AreaValue { get; set; }
        [Column("release_status")]
        public int? ReleaseStatus { get; set; }
        [Column("section_status")]
        public int? SectionStatus { get; set; }
        [Column("section_type")]
        public int? SectionType { get; set; }
        [Column("usage_fee")]
        public int? UsageFee { get; set; }
        [Column("management_fee")]
        public int? ManagementFee { get; set; }
        [Column("monument_cost")]
        public int? MonumentCost { get; set; }
        [Column("set_price")]
        public int? SetPrice { get; set; }
        [Column("total_price")]
        public int? TotalPrice { get; set; }
        [Column("create_date")]
        public DateTime? CreateDate { get; set; }
        [Column("update_date")]
        public DateTime? UpdateDate { get; set; }
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