using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SurveyProject.Models
{
    public partial class Questionnaires
    {
        public Questionnaires()
        {
            Questions = new HashSet<Questions>();
            UserQuestionnaire = new HashSet<UserQuestionnaire>();
        }

        public Guid Id { get; set; }
        public string Titel { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<Questions> Questions { get; set; }
        public virtual ICollection<UserQuestionnaire> UserQuestionnaire { get; set; }
    }
}
