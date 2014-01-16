using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IsThereAList.Models
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            this.Days = Enumerable.Range(1, 31).ToArray();
            this.Months = new List<Month>
                                {
                                    new Month(1, "Jan"),
                                    new Month(2, "Feb"),
                                    new Month(3, "Mar"),
                                    new Month(4, "Apr"),
                                    new Month(5, "May"),
                                    new Month(6, "Jun"),
                                    new Month(7, "Jul"),
                                    new Month(8, "Aug"),
                                    new Month(9, "Sep"),
                                    new Month(10, "Oct"),
                                    new Month(11, "Nov"),
                                    new Month(12, "Dec"),
                                };
        }

        [Display(Name = "User")]
        public ApplicationUser EditableObject { get; set; }

        public int[] Days { get; set; }
        public IEnumerable<Month> Months { get; set; }
    }

    public class InternalRegisterViewModel : RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalRegisterViewModel : RegisterViewModel
    {
        public string LoginProvider { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ExternalLoginViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class Month
    {
        public Month() { }
        public Month(int number, string name)
        {
            this.Number = number;
            this.Name = name;
        }
        public int Number { get; set; }
        public string Name { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
