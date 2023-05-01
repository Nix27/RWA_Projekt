using DAL.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class VideoVM
    {
        public VideoDto Video { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Genres { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Images { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> Tags { get; set; }
    }
}
