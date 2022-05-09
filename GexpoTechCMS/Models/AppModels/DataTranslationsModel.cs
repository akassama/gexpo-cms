using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* DATA TRANSLATIONS */
    [Table("DataTranslations")]
    public class DataTranslationsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Document ID")]
        [StringLength(255)]
        public string DocumentID { get; set; }

        [Display(Name = "Document Model")]
        [StringLength(255)]
        public string DocumentModel { get; set; }

        [Display(Name = "Language")]
        public string Language { get; set; }

        [Display(Name = "Translation Title")]
        public string TranslationTitle { get; set; }

        [Display(Name = "Translation Description")]
        public string TranslationDescription { get; set; }

        [Display(Name = "Translation Content")]
        public string TranslationContent { get; set; }

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
