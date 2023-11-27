using AccesData.DTOs.RentBookDTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using Services.Interfaces;

namespace Services.RentBookService
{
    public class RentBookService : IRentBookService
    {
        private readonly IRentBookRepository _rentBookRepository;

        private readonly IBookRepository _bookRepository;

        public RentBookService(IRentBookRepository rentBookRepository, IBookRepository bookRepository)
        {
            _rentBookRepository = rentBookRepository;
            _bookRepository = bookRepository;
        }

        public async Task<CreateRentBookDTO> CreateRentBookService(CreatRentBookControlleRequest rentBookRequest)
        {
            try
            {

                // se crea la fecha de vencimiento del alquiler, 5 dias despues de la fecha de entrega
                DateTime rentExpirationDate = rentBookRequest.rentDate.AddDays(5);
                long rentExpirationDateEpoch = new DateTimeOffset(rentExpirationDate).ToUnixTimeMilliseconds();
                //la fecha de entrega del libro se pasa a milisegundos(epoch)
                long rentDateEpoch = new DateTimeOffset(rentBookRequest.rentDate).ToUnixTimeMilliseconds();
                //buscar el id del libro por el nombre
                int idBookFound = await _bookRepository.DataGetIdBook(rentBookRequest.name_book);
                if (idBookFound == 0) return new CreateRentBookDTO { msg = "book not found" };
                //creacion modelo del nuevo alquiler para la base de datos
                var newRentBook = new CreateRentBookRequest { id_book = idBookFound, id_user = rentBookRequest.id_user, rent_date_epoch = rentDateEpoch, expiration_date_epoch = rentExpirationDateEpoch };

                CreateRentBookDTO newUser = await _rentBookRepository.DataCreateRentBook(newRentBook);
                if (newUser.msg == "error database") return new CreateRentBookDTO { msg = "server error" };
                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service error {ex.Message}");
                return new CreateRentBookDTO { msg = "server error" };
            }


        }


    }
}
