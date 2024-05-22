using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Models
{
    public class Cemetery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("cemetery_index")]
        
        public int CemeteryIndex { get; set; }

        [Column("section_Index")]
        public int SectionIndex { get; set; }

        [Column("cemetery_code")]
        public string Cemetery_Code { get; set; }

        [Column("cemetery_name")]
        public string Cemetery_Name { get; set; }
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
}
