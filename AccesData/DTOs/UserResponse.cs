

namespace AccesData.DTOs
{
    public class UserResponse
    {
        public int id_user { get; set; }
        public string name_user { get; set; }
        public string mail_user { get; set; }
        public string msg { get; set; }
        public bool result { get; set; }

    }

    public class GetUserByIdResponse
    {
        public int id_user { get; set; }
        public string name_user { get; set; }
        public string mail_user { get; set; }
        public string msg { get; set; }
        public bool result { get; set; }

    }
    public class CreateUserResponse
    {
        public int id_user { get; set; }
        public string name_user { get; set; }
        public string mail_user { get; set; }
        public bool result { get; set; }
        public string msg { get; set; }

    }
}
