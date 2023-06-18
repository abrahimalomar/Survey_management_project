using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SurveyProject.Models
{
    public partial class UserQuestionnaire
    {
        public UserQuestionnaire()
        {
            ListQuestions = new HashSet<Questions>();
        }
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public string answer { get; set; }
        public string UserId { get; set; }
        public Guid? Questionnaireid { get; set; }
        public Guid? QutionId { get; set; }
        [NotMapped]
        public virtual ICollection<Questions> ListQuestions { get; set; }
        public virtual Questions Questions { get; set; }
        public virtual Questionnaires Questionnaire { get; set; }
        public virtual AspNetUsers User { get; set; }

        
    }
}
