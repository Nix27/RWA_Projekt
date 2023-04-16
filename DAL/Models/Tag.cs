using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name of tag is required")]
        public string Name { get; set; }
        public ICollection<VideoTag> VideoTags { get; set; }
    }
}
