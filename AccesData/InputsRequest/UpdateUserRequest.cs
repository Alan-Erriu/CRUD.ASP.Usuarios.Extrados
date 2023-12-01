using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class UpdateUserRequest
    {
        [Required]
        public int id_user { get; set; }
        [Required]
        public string name_user { get; set; }

    }
}
