using AccesData.DTOs;
using AccesData.Models;

namespace AccesData.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> DataLogin(LoginRequestDTO loginRequestDTO);
    }
}