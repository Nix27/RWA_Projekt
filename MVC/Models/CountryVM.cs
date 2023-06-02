using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class CountryVM
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        [StringLength(3, MinimumLength = 3, ErrorMessage = "Country code must have 3 characters")]
        public string Code { get; set; }
    }
}
