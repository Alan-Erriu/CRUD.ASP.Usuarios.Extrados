using AccesData.DTOs;
using AccesData.InputsRequest;

namespace Services.Interfaces
{
    public interface IUserService
    {

        Task<CreateUserWithRoleDTO> CreateUserWithRoleService(CreateUserWithRoleRequest createUserRequest);
        Task<int> DeleteUserByIdService(int id);
        Task<GetUserByIdDTO> GetUserByIdProtectedService(GetUserByIdRequest request);
        Task<GetUserByIdDTO> GetUserByIdService(int id_user);
        Task<int> UpdateUserByIdService(UpdateUserRequest updateUserRequestDTO);

        bool IsValidEmail(string email);
    }
}