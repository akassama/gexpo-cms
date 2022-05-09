using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* HOME SLIDERS MODEL */
    [Table("HomeSliders")]
    public class HomeSliderModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Slider ID")]
        [StringLength(255)]
        public string SliderID { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(255)]
        public string SliderTitle { get; set; }

        [Display(Name = "Sub Text")]
        [StringLength(500)]
        public string SubText { get; set; }

        [Display(Name = "Slider Link")]
        [StringLength(255)]
        public string SliderLink { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "Slider Image")]
        public string SliderImage { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
