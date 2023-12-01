using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class CreatRentBookControlleRequest
    {
        [Required]
        public int id_user { get; set; }

        [Required]
        public string name_book { get; set; }

        [Required]

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime rentDate { get; set; }
    }
}
