using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccesData.InputsRequest
{
    public class CreateUserWithRoleRequest
    {

        public string name_user { get; set; }
        public string mail_user { get; set; }
        public string password_user { get; set; }
        public string role_user { get; set; }

    }
}
