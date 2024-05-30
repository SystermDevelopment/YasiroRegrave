using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YasiroRegrave.Model;

[Table("m_survey")]

public class Surveys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("survey_index")]

        public int SurveyIndex { get; set; }

        [Column("survey")]
        [MaxLength(100)]
        public string Survey { get; set; }
    }

