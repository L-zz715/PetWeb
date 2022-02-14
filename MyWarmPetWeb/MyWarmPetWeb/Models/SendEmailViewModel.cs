using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWarmPetWeb.Models
{
    public class SendEmailViewModel
    {
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Please enter a subject.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter the contents")]
        public string Contents { get; set; }

        public HttpPostedFileBase Upload { get; set; }
    }
}