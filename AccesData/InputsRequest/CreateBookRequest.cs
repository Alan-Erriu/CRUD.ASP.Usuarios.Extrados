﻿using System.ComponentModel.DataAnnotations;

namespace AccesData.InputsRequest
{
    public class CreateBookRequest
    {
        [Required]
        public string name_book { get; set; }
    }
}
