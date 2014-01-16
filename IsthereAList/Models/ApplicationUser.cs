using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IsThereAList.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        private string _emailAddress = String.Empty;

        public ApplicationUser()
        {
            this.SendEmails = true;
        }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        [Display(Name = "Email address")]
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                _emailAddress = value ?? String.Empty;
                //var tmp = _emailAddress.Replace("@", "at").Replace(".", "dot").Replace("_", "und");
                //this.UserName = Regex.Replace(tmp, "[^0-9a-zA-Z]+", "");
                this.UserName = _emailAddress;
            }
        }

        [Required]
        [Display(Name = "Send email notifications")]
        public bool SendEmails { get; set; }

        [Range(1, 31)]
        [Required]
        [Display(Name = "DoB Day")]
        public int DobDay { get; set; }

        [Range(1, 12)]
        [Required]
        [Display(Name = "DoB Month")]
        public int DobMonth { get; set; }
    }
}