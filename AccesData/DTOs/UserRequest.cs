

namespace AccesData.DTOs
{


    public class GetUserByIdRequest
    {
        public int id_user { get; set; }

        public string password_user { get; set; }

    }
    public class UpdateUserByIdRequest
    {
        public int id_user { get; set; }

        public string name_user { get; set; }

    }

    public class CreateUserRequest
    {

        public string name_user { get; set; }

        public string mail_user { get; set; }

        public string password_user { get; set; }
    }


}
