using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_section")]
public class Section
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("section_index")]
        public int SectionIndex { get; set; }

        [Column("area_index")]
        public int AreaIndex { get; set; }

        [Column("section_code")]
        [MaxLength(100)]
        public string SectionCode { get; set; }

        [Column("section_name")]
        [MaxLength(100)]
        public string SectionName { get; set; }

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
        [ForeignKey("area_Index")]
        public virtual Area Area { get; set; } = new Area();

    }

