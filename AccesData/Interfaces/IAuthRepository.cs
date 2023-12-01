using AccesData.DTOs;
using AccesData.DTOs.AuthDTOs;
using AccesData.InputsRequest;
using AccesData.Models;

namespace AccesData.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> DataSignIn(LoginRequest loginRequestDTO);
        Task<CreateUserDTO> DataSignUp(CreateUserRequest LoginRequestDTO);
        Task<LoginDTO> DataInsertRefreshToken(TokenHistory tokenRequest);
        Task<RefreshTokenDTO> DataSelectRefreshToken(int id_user);
        Task<int> DataDeleteRefreshTokenExpired(int id_user);




    }
}