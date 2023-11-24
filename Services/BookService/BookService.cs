using AccesData.DTOs.BookDTOs;
using AccesData.Interfaces;
using Services.Interfaces;

namespace Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<CreateBookDTO> CreateBookService(string nameBook)
        {
            try
            {
                var bookAlreadyExists = await _bookRepository.DataCompareNameBook(nameBook);

                if (bookAlreadyExists != null) return new CreateBookDTO { msg = "The name is already in use" };

                CreateBookDTO newBook = await _bookRepository.DataCreateBook(nameBook);

                if (newBook.msg == "error database") return new CreateBookDTO { msg = "server error" };

                return new CreateBookDTO { id_book = newBook.id_book, name_book = newBook.name_book, msg = "ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error {ex.Message}");
                return new CreateBookDTO { msg = "server error" };
            }
        }





    }
}