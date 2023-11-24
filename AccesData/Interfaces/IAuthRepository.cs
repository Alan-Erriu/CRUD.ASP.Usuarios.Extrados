using AccesData.DTOs;
using AccesData.InputsRequest;
using AccesData.Models;

namespace AccesData.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> DataSignIn(LoginRequest loginRequestDTO);


    }
}