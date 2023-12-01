using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class CreateRoleRequest
    {
        [Required]
        public string name_role { get; set; }
        [Required]
        public string description_role { get; set; }
    }
}
