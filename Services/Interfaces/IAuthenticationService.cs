using AccesData.DTOs;
using AccesData.InputsRequest;

namespace Services.Interfaces
{
    public interface IAuthenticationService
    {


        Task<LoginDTO> SignInService(LoginRequest LoginRequestDTO);
        Task<CreateUserDTO> SignUpService(CreateUserRequest createUserRequest);
        Task<LoginDTO> ReturnRefreshToken(RefreshTokenRequest TokenRequest);
        CreateUserWithRoleDTO GetUserFromExpiredToken(string token);
        Task<bool> RefreshTokenIsActive(int id_user);
        Task<int> DeleteRefreshTokenExpiredFromBd(int id_user);
        Task<bool> CompareRefreshTokens(int id_user, string refreshToken);

    }
}