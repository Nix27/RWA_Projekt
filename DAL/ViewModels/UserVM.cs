using DAL.DTO;
using DAL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class UserVM
    {
        public UserDto User { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Roles { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
