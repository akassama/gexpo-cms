using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* PAGE SECTIONS MODEL */
    [Table("PageSections")]
    public class PageSectionsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Section ID")]
        [StringLength(255)]
        public string SectionID { get; set; }

        [Display(Name = "Section Name")]
        [StringLength(255)]
        public string SectionName { get; set; }

        [Display(Name = "Section Title")]
        public string SectionTitle { get; set; }

        [Display(Name = "Content")]
        public string SectionContent { get; set; }

        [Display(Name = "Section Type")]
        [StringLength(100)]
        public string SectionType { get; set; }

        [Display(Name = "Order")]
        public int SectionOrder { get; set; } = 10;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
