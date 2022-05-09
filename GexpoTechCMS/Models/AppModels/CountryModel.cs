using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* COUNTRIES MODEL */
    [Table("Country")]
    public class CountryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "ISO")]
        public string ISO { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Nice Name")]
        public string NiceName { get; set; }

        [Display(Name = "ISO3")]
        public string ISO3 { get; set; }

        [Display(Name = "Num Code")]
        public Int16? NumCode { get; set; }

        [Display(Name = "Phone Code")]
        public int? PhoneCode { get; set; }
    }
}
