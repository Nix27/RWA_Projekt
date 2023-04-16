using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name of genre is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}
