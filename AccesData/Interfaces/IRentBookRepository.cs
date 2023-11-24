using AccesData.DTOs.RentBookDTOs;
using AccesData.InputsRequest;

namespace AccesData.Interfaces
{
    public interface IRentBookRepository
    {
        Task<CreateRentBookDTO> DataCreateRentBook(CreateRentBookRequest rentBookRequest);
    }
}