using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace SurveyProject.Models
{
    public partial class Options
    {
        public Guid Id { get; set; }
        [Display(Name = "Option")]
        public string Options1 { get; set; }
        public OptionType Type { get; set; }
        public Guid? QutionId { get; set; }
        [Display(Name = "Question text")]
        public virtual Questions Qution { get; set; }
    }
    public enum OptionType
    {
        radio,
        checkbox
    }
}
