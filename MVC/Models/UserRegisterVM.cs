using BL.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class UserRegisterVM
    {
        public UserRegisterRequest UserForRegister { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Roles { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
