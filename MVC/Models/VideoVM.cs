using BL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class VideoVM
    {
        public VideoDto Video { get; set; }

        [ValidateNever]
        public IFormFile Image { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Genres { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Tags { get; set; }

        [ValidateNever]
        public string ImageURL { get; set; }

        [ValidateNever]
        public string Genre { get; set; }

        [ValidateNever]
        public IEnumerable<string> TagsOfVideo { get; set; }
    }
}
