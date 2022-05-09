using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* MESSAGE VIEWS MODEL */
    [Table("MessageViews")]
    public class MessageViewModel
    {
        [Key]
        [Display(Name = "View ID")]
        [StringLength(255)]
        public string ViewID { get; set; }

        [Display(Name = "Contact ID")]
        [StringLength(255)]
        public string ContactID { get; set; }

        [Display(Name = "Account ID")]
        [StringLength(255)]
        public string AccountID { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
