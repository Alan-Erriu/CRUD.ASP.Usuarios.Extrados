using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class CreateUserRequest
    {
        [Required]
        public string name_user { get; set; }
        [Required]
        public string mail_user { get; set; }
        [Required]
        public string password_user { get; set; }


    }
}
