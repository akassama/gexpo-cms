using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* TEAM MODEL */
    [Table("Team")]
    public class TeamModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Team ID")]
        [StringLength(255)]
        public string TeamID { get; set; }

        [Display(Name = "First Name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Title")]
        [StringLength(255)]
        public string Title { get; set; }

        [Display(Name = "Profile Image")]
        [StringLength(250)]
        public string ProfileImage { get; set; }

        [Display(Name = "Link")]
        [StringLength(255)]
        public string Link { get; set; }

        [Display(Name = "Facebook")]
        [StringLength(255)]
        public string Facebook { get; set; }

        [Display(Name = "Twitter")]
        [StringLength(255)]
        public string Twitter { get; set; }

        [Display(Name = "Instagram")]
        [StringLength(255)]
        public string Instagram { get; set; }

        [Display(Name = "LinkedIn")]
        [StringLength(255)]
        public string LinkedIn { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
