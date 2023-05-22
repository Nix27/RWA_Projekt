using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class ChangePasswordVM
    {
        public string Email { get; set; }

        [DisplayName("Old password")]
        public string OldPassword { get; set; }

        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [ValidateNever]
        public int UserId { get; set; }
    }
}
