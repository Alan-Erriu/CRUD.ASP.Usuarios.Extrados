using AccesData.DTOs;

namespace Services.Interfaces
{
    public interface IAuthenticationService
    {
        string CreateToken(string id_user);
        Task<LoginDTO> ReturnToken(LoginRequestDTO LoginRequestDTO);
    }
}