using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Content of image is required")]
        public string? Content { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}
