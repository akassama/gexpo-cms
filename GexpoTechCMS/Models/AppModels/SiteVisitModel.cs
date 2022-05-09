using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* SITE VISITS MODEL */
    [Table("SiteVisits")]
    public class SiteVisitModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Visit ID")]
        [StringLength(255)]
        public string VisitID { get; set; }

        [Display(Name = "Document ID")]
        [StringLength(255)]
        public string DocumentID { get; set; }

        [Display(Name = "Document Type")]
        public string DocumentType { get; set; }

        [Display(Name = "Ip Address")]
        public string IpAddress { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Browser")]
        public string Browser { get; set; }

        [Display(Name = "Device")]
        public string Device { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
