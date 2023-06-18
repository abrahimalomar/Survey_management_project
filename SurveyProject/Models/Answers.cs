using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SurveyProject.Models
{
    public partial class Answers
    {
        public Guid Id { get; set; }
        public string Answer { get; set; }
        public Guid? QutionsId { get; set; }
        public string UserId { get; set; }

        public virtual Questions Qutions { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
