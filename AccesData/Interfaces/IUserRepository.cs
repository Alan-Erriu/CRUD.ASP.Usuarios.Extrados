using AccesData.DTOs;
using AccesData.Models;

namespace AccesData.Interfaces
{
    public interface IUserRepository
    {
        Task<User> DataCreateUser(CreateUserRequestDTO newUser);
        Task<int> DataDeleteUserById(int id);
        Task<User> DataGetUserByID(int id_user);
        Task<int> DataUpdateUserById(UpdateUserRequestDTO updateUserRequestDTO);
        Task<User> DataCompareEmailUserByMail(string mail_user);
    }
}