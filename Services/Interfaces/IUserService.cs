using AccesData.DTOs;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserDTO> CreateUserService(CreateUserRequestDTO createUserRequest);
        Task<int> DeleteUserByIdService(int id);
        Task<GetUserByIdDTO> GetUserByIdProtectedService(GetUserByIdRequestDTO request);
        Task<GetUserByIdDTO> GetUserByIdService(int id_user);
        Task<int> UpdateUserByIdService(UpdateUserRequestDTO updateUserRequestDTO);

        bool IsValidEmail(string email);
    }
}