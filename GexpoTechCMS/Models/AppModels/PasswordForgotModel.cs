using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* CHANGE PASSWORD MODEL */
    [Table("PasswordForgot")]
    public class PasswordForgotModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Account ID")]
        public string AccountID { get; set; }

        [Required]
        [Display(Name = "Reset ID")]
        public string ResetID { get; set; }

        [Required]
        [Display(Name = "ResetDate")]
        public DateTime? ResetDate { get; set; } = DateTime.Now;
    }
}
