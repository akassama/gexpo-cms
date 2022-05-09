using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* CONFIGURATIONS MODEL */
    [Table("Configurations")]
    public class ConfigurationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Configuration ID")]
        [StringLength(255)]
        public string ConfigurationID { get; set; }

        [Display(Name = "Configuration Key")]
        [StringLength(100)]
        public string ConfigurationKey { get; set; }

        [Display(Name = "Configuration Value")]
        public string ConfigurationValue { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
