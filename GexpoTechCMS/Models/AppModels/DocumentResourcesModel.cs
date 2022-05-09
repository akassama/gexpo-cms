using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* DOCUMENT RESOURCES MODEL */
    [Table("DocumentResources")]
    public class DocumentResourcesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Document ID")]
        [StringLength(255)]
        public string DocumentID { get; set; }

        [Display(Name = "Page ID")]
        [StringLength(255)]
        public string PageID { get; set; }

        [Display(Name = "Document Name")]
        [StringLength(255)]
        public string DocumentName { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(500)]
        public string ShortDescription { get; set; }

        [Display(Name = "File Name")]
        [StringLength(255)]
        public string FileName { get; set; }

        [Display(Name = "File Extension")]
        [StringLength(255)]
        public string FileExtension { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
