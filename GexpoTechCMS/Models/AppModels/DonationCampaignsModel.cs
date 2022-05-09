using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* DONATION CAMPAIGNS */
    [Table("DonationCampaigns")]
    public class DonationCampaignsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Donation ID")]
        [StringLength(255)]
        public string DonationID { get; set; }

        [Display(Name = "Campaign Title")]
        [StringLength(255)]
        public string CampaignTitle { get; set; }

        [Display(Name = "Description")]
        public string CampaignDescription { get; set; }

        [Display(Name = "Image")]
        [StringLength(255)]
        public string CampaignImage { get; set; }

        [Display(Name = "Type")]
        [StringLength(100)]
        public string DonationType { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
