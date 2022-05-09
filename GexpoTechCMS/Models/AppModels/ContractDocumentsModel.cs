using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* ContractDocuments MODEL */
    [Table("ContractDocuments")]
    public class ContractDocumentsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Contract ID")]
        [StringLength(255)]
        public string ContractID { get; set; }

        [Display(Name = "File Name")]
        [StringLength(255)]
        public string FileName { get; set; }

        [Display(Name = "File Type")]
        [StringLength(255)]
        public string FileType { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
