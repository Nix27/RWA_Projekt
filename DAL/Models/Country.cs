using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Country code must have 3 characters")]
        public string Code { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
