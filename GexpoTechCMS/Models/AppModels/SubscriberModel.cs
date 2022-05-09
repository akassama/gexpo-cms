using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* SUBSCRIBERS MODEL */
    [Table("Subscribers")]
    public class SubscriberModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; } 

        [Display(Name = "Created At")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
