using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class GetUserByIdRequest
    {
        [Required]
        public int id_user { get; set; }
        [Required]
        public string password_user { get; set; }
    }
}
