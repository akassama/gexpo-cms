using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* ACCOUNTS MODEL */
    [Table("Accounts")]
    public class AccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Account ID")]
        [StringLength(255)]
        public string AccountID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Gender")]
        [StringLength(50)]
        public string Gender { get; set; }

        [Display(Name = "Country")]
        [StringLength(100)]
        public string Country { get; set; }

        [Display(Name = "Address")]
        [StringLength(255)]
        public string Address { get; set; }

        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,}", ErrorMessage = "Must contain at least one number and one uppercase and lowercase letter, and at least 6 or more characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(500)]
        public string Password { get; set; }

        [Display(Name = "Profile Picture")]
        [StringLength(255)]
        public string ProfilePicture { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; } = false;

        [Display(Name = "Oauth")]
        public bool Oauth { get; set; } = false;

        [Display(Name = "Email Verification")]
        public bool EmailVerification { get; set; }

        [Display(Name = "Email Notification")]
        public bool EmailNotification { get; set; }

        [Display(Name = "Directory Name")]
        [StringLength(255)]
        public string DirectoryName { get; set; }

        [Display(Name = "Session Key")]
        [StringLength(500)]
        public string SessionKey { get; set; }

        [Display(Name = "Remember Token")]
        [StringLength(500)]
        public string RememberToken { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
