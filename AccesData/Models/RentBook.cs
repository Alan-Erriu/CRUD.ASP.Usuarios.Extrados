namespace AccesData.Models
{
    public class RentBook
    {

        public int id_rent_book { get; set; }

        public int id_book { get; set; }

        public string id_user { get; set; }

        public long rent_date_epoch { get; set; }

        public long expiration_date_epoch { get; set; }


    }
}
