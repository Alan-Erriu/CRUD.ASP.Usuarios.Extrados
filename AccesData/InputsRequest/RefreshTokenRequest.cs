using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class RefreshTokenRequest
    {
        [Required]
        public string expiredToken { get; set; }




    }
}
