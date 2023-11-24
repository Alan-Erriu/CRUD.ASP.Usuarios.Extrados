using AccesData.DTOs.BookDTOs;

namespace AccesData.Interfaces
{
    public interface IBookRepository
    {
        Task<CreateBookDTO> DataCreateBook(string nameBook);

        Task<string> DataCompareNameBook(string nameBook);

        Task<int> DataGetIdBook(string nameBook);
    }
}