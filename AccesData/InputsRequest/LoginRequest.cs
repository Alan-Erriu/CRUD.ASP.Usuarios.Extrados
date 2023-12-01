using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class LoginRequest
    {
        [Required]
        public string mail_user { get; set; }
        [Required]
        public string password_user { get; set; }
    }
}
