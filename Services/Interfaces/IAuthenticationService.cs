using AccesData.DTOs;
using AccesData.InputsRequest;

namespace Services.Interfaces
{
    public interface IAuthenticationService
    {
        string CreateToken(string id_user, string name_user, string mail_user, string role_user);
        Task<LoginDTO> SignInService(LoginRequest LoginRequestDTO);
        Task<CreateUserDTO> SignUpService(CreateUserRequest createUserRequest);

    }
}