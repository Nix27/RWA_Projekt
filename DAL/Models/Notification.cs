using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Email of receiver is required")]
        public string ReceiverEmail { get; set; }
        public string? Subject { get; set; }
        [Required(ErrorMessage = "Body of email is required")]
        public string Body { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
