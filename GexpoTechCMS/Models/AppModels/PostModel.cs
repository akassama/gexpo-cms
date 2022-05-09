using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    /* POSTS MODEL */
    [Table("Posts")]
    public class PostModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Post ID")]
        [StringLength(255)]
        public string PostID { get; set; }

        [Display(Name = "Post Type")]
        [StringLength(100)]
        public string PostType { get; set; }

        [Display(Name = "Slug")]
        [StringLength(250, MinimumLength = 5)]
        public string Slug { get; set; }

        [Display(Name = "Author")]
        [StringLength(255)]
        public string Author { get; set; }

        [Display(Name = "Categories")]
        [StringLength(255)]
        public string Categories { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Short Description")]
        [StringLength(500)]
        public string ShortDescription { get; set; }

        [Display(Name = "Post Image")]
        public string PostImage { get; set; }

        [Display(Name = "Image Caption")]
        [StringLength(500)]
        public string ImageCaption { get; set; }

        [Display(Name = "Featured Post?")]
        public bool FeaturedPost { get; set; } = false;

        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Post Tags")]
        [StringLength(500)]
        public string PostTags { get; set; }

        [Display(Name = "Status")]
        [RegularExpression(@"^[0-1]$", ErrorMessage = "Only numbers allowed. 0 or 1")]
        public int Status { get; set; } = 0;

        [Display(Name = "SEO Title")]
        [StringLength(250)]
        public string SEOTitle { get; set; }

        [Display(Name = "SEO Description")]
        [StringLength(250)]
        public string SEODescription { get; set; }

        [Display(Name = "SEO Keywords")]
        [StringLength(250)]
        public string SEOKeywords { get; set; }

        [Display(Name = "UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "CreatedAt")]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
