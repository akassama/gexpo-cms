using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* APP MESSAGES MODEL */
    [Table("AppMessages")]
    public class AppMessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Name")]
        [StringLength(255)]
        public string MessageName { get; set; }

        [Display(Name = "Message Key")]
        [StringLength(255)]
        public string MessageKey { get; set; }

        [Display(Name = "MessageValue")]
        public string MessageValue { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
