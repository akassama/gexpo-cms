using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* THEME SETTINGS MODEL */
    [Table("ThemeSettings")]
    public class ThemeSettingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Theme ID")]
        [StringLength(255)]
        public string ThemeSettingID { get; set; }

        [Display(Name = "Setting Name")]
        [StringLength(100)]
        public string SettingName { get; set; }

        [Display(Name = "Setting Type")]
        [StringLength(100)]
        public string SettingType { get; set; }

        [Display(Name = "Setting Value")]
        [StringLength(500)]
        public string SettingValue { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
