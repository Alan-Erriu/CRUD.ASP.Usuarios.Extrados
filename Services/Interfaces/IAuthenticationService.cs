using AccesData.DTOs;

namespace Services.Interfaces
{
    public interface IAuthenticationService
    {
        string CreateToken(string id_user, string name_user, string mail_user);
        Task<LoginDTO> ReturnToken(LoginRequestDTO LoginRequestDTO);
    }
}