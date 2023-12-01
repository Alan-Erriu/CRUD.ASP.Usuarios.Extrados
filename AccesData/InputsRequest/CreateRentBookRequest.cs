using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class CreateRentBookRequest
    {
        [Required]
        public int id_book { get; set; }
        [Required]
        public int id_user { get; set; }
        [Required]
        public long rent_date_epoch { get; set; }
        [Required]
        public long expiration_date_epoch { get; set; }
    }
}
