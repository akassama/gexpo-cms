using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* CATEGORY MODEL */
    [Table("Category")]
    public class CategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Category ID")]
        [StringLength(255)]
        public string CategoryID { get; set; }

        [Display(Name = "Category Name")]
        [StringLength(255)]
        public string CategoryName { get; set; }

        [Display(Name = "Short Category Name")]
        [StringLength(255)]
        public string ShortCategoryName { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(255)]
        public string ShortDescription { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "Icon")]
        [StringLength(255)]
        public string Icon { get; set; }

        [Display(Name = "Order")]
        public int Order { get; set; } = 10;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
