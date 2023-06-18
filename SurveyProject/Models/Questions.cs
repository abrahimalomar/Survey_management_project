using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SurveyProject.Models
{
    public partial class Questions
    {
        public Questions()
        {
            Answers = new HashSet<Answers>();
            Options = new HashSet<Options>();
        
        }

        public Guid Id { get; set; }
        [Display(Name = "Question text")]
        public string QuestionTitile { get; set; }
        public QutionType Type { get; set; }
        public bool IsRequired { get; set; }
        public Guid? QuestionnaireId { get; set; }

        public virtual Questionnaires Questionnaire { get; set; }
        public virtual ICollection<Answers> Answers { get; set; }
        public virtual ICollection<Options> Options { get; set; }
        public virtual ICollection<UserQuestionnaire> UserQuestionnaires { get; set; }
       
         
    }
    public enum QutionType
    {
        Null,
        radio,
        number,
        text,
        date,
        email
    }
}
