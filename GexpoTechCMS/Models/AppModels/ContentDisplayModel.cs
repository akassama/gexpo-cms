using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* CONTENT DISPLAY MODEL */
    [Table("ContentDisplay")]
    public class ContentDisplayModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Content ID")]
        [StringLength(255)]
        public string ContentDisplayID { get; set; }

        [Display(Name = "Content Name")]
        [StringLength(255)]
        public string ContentName { get; set; }

        [Display(Name = "Display Name")]
        public string ContentDisplayName { get; set; }

        [Display(Name = "Display Status")]
        public bool DisplayStatus { get; set; } = false;

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
