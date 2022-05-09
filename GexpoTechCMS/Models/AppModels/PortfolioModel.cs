using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* PORTFOLIO MODEL */
    [Table("Portfolio")]
    public class PortfolioModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Portfolio ID")]
        [StringLength(255)]
        public string PortfolioID { get; set; }

        [Display(Name = "Title")]
        [StringLength(255)]
        public string PortfolioTitle { get; set; }

        [Display(Name = "Slug")]
        [StringLength(250, MinimumLength = 5)]
        public string Slug { get; set; }

        [Display(Name = "Image")]
        [StringLength(250)]
        public string Image { get; set; }

        [Display(Name = "Category")]
        [StringLength(250)]
        public string Category { get; set; }

        [Display(Name = "Content")]
        public string PortfolioContent { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
