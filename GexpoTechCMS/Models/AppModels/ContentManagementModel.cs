using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* CONTENT MANAGEMENT MODEL */
    [Table("ContentManagement")]
    public class ContentManagementModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Content ID")]
        [StringLength(255)]
        public string ContentID { get; set; }

        [Display(Name = "Content Name")]
        [StringLength(100)]
        public string ContentName { get; set; }

        [Display(Name = "Content Group")]
        [StringLength(100)]
        public string ContentGroup { get; set; }

        [Display(Name = "Content Value")]
        public string ContentValue { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
