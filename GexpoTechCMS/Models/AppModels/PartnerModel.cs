using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* PARTNER MODEL */
    [Table("Partner")]
    public class PartnerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Partner ID")]
        [StringLength(255)]
        public string PartnerID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(255)]
        public string PartnerName { get; set; }

        [Display(Name = "Logo")]
        [StringLength(250)]
        public string Logo { get; set; }

        [Display(Name = "Link")]
        [StringLength(255)]
        public string Link { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
