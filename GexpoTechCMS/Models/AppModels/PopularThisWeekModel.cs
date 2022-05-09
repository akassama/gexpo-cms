using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* POPULAR THIS WEEK MODEL */
    [Table("PopularThisWeek")]
    public class PopularThisWeekModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Document ID")]
        public string DocumentID { get; set; }

        [Display(Name = "Value Occurrence")]
        public int ValueOccurrence { get; set; }
    }
}
