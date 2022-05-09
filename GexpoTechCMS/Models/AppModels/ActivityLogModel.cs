using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* ACTIVITY LOGS MODEL */
    [Table("ActivityLogs")]
    public class ActivityLogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Activity ID")]
        public string ActivityID { get; set; }

        [Display(Name = "Activity By")]
        public string ActivityBy { get; set; }

        [Display(Name = "Type")]
        public string ActivityType { get; set; }

        [Required]
        [Display(Name = "Activity")]
        public string Activity { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; }
    }
}
