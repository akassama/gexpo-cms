using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* LANGUAGES */
    [Table("Languages")]
    public class LanguagesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "ISO")]
        [StringLength(255)]
        public string ISO { get; set; }
    }
}
