using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* TESTIMONIAL MODEL */
    [Table("Testimonials")]
    public class TestimonialModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Testimonial ID")]
        [StringLength(255)]
        public string TestimonialID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(250)]
        public string Title { get; set; }

        [Display(Name = "Organization")]
        [StringLength(255)]
        public string Organization { get; set; }

        [Display(Name = "ProfileImage")]
        [StringLength(255)]
        public string ProfileImage { get; set; }

        [Required]
        [Display(Name = "Testimonial")]
        public string Testimonial { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
