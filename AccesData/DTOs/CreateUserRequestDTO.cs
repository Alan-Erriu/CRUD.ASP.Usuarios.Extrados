using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesData.DTOs
{
    public class CreateUserRequestDTO
    {
        public int id_user { get; set; }
        public string name_user { get; set; }
        public string mail_user { get; set; }
        public string password_user { get; set; }
        public string msg { get; set; }
        public bool result { get; set; }
    }
}
