using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ValidateEmilRequest
    {
        public string Email { get; set; }
        public string B64SecToken { get; set; }
    }
}
