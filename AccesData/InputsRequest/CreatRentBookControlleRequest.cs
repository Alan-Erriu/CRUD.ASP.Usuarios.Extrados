using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class CreatRentBookControlleRequest
    {
        [Required(ErrorMessage = "User ID is required")]
        public int id_user { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string name_book { get; set; }

        [Required(ErrorMessage = "Rent date is required")]

        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        public DateTime rentDate { get; set; }
    }
}
