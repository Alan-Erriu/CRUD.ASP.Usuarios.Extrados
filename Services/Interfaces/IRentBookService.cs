using AccesData.DTOs.RentBookDTOs;
using AccesData.InputsRequest;

namespace Services.Interfaces
{
    public interface IRentBookService
    {
        Task<CreateRentBookDTO> CreateRentBookService(CreatRentBookControlleRequest rentBookRequest);
    }
}