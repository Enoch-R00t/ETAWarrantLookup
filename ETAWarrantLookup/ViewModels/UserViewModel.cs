using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETAWarrantLookup.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "User name required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Business Name")]
        public string BusinessName { get; set; }

        [Phone]
        [Required(ErrorMessage = "Phone number required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirm Password required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
