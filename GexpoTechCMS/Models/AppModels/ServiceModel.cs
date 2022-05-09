using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{

    /* SERVICE MODEL */
    [Table("Services")]
    public class ServiceModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Service ID")]
        [StringLength(255)]
        public string ServiceID { get; set; }

        [Display(Name = "Title")]
        [StringLength(255)]
        public string ServiceTitle { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(500)]
        public string ShortDescription { get; set; }

        [Display(Name = "Link")]
        [StringLength(255)]
        public string ServiceLink { get; set; }

        [Display(Name = "Icon")]
        [StringLength(255)]
        public string ServiceIcon { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
