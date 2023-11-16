using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesData.DTOs
{
    public class LoginDTO
    {
        public string token { get; set; }
        public string msg { get; set; }
        public bool result { get; set; }
    }
}
