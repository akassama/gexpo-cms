using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* SERVICE STAT MODEL */
    [Table("ServiceStats")]
    public class ServiceStatModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "ServiceStat ID")]
        [StringLength(255)]
        public string ServiceStatID { get; set; }

        [Display(Name = "Title")]
        [StringLength(255)]
        public string Title { get; set; }

        [Display(Name = "Stat Value")]
        public string StatValue { get; set; }

        [Display(Name = "Icon")]
        public string Icon { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Order")]
        public int Order { get; set; } = 10;

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
