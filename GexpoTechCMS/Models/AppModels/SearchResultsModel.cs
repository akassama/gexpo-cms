using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GexpoTechCMS.Models.AppModels
{
    public class SearchResultsModel
    {

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
