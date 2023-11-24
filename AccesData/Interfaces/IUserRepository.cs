using AccesData.DTOs;
using AccesData.InputsRequest;
using AccesData.Models;

namespace AccesData.Interfaces
{
    public interface IUserRepository
    {
        Task<CreateUserDTO>DataCreateUser(CreateUserRequest newUser);
        Task<int> DataDeleteUserById(int id);
        Task<User> DataGetUserByID(int id_user);
        Task<int> DataUpdateUserById(UpdateUserRequest updateUserRequestDTO);
        Task<User> DataCompareEmailUserByMail(string mail_user);
        Task<CreateUserWithRoleDTO> DataCreateUserWithRole(CreateUserWithRoleRequest newUser);

    }
}