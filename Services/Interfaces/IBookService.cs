﻿using AccesData.DTOs.BookDTOs;

namespace Services.Interfaces
{
    public interface IBookService
    {
        Task<CreateBookDTO> CreateBookService(string nameBook);
    }
}