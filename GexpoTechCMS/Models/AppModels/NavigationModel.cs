using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{

    /* NAVIGATION MODEL */
    [Table("Navigation")]
    public class NavigationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Navigation ID")]
        [StringLength(255)]
        public string NavigationID { get; set; }

        [Display(Name = "Name")]
        [StringLength(100)]
        public string NavigationName { get; set; }

        [Display(Name = "Link")]
        [StringLength(255)]
        public string NavigationLink { get; set; }

        [Display(Name = "Order")]
        public int Order { get; set; } = 10;

        [Display(Name = "Parent")]
        [StringLength(100)]
        public string Parent { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
