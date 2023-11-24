﻿namespace AccesData.InputsRequest
{
    public class CreateRentBookRequest
    {
        public int id_book { get; set; }

        public int id_user { get; set; }

        public long rent_date_epoch { get; set; }

        public long expiration_date_epoch { get; set; }
    }
}
