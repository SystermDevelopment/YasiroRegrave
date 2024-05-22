using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("t_survey")]

public class Survey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("survey_link_index")]

        public int SurveyLinkIndex { get; set; }

        [Column("survey_index")]
        public int SurveyIndex { get; set; }

        [Column("reserve_index")]
        public int ReserveIndex { get; set; }
        [ForeignKey("SurveyLinkIndex")]
        public virtual Survey Surveys { get; set; } = new Survey();
}

