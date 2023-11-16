using AccesData.DTOs;
using AccesData.Interfaces;
using AccesData.Models;
using AccesData.Repositories;
using Dapper;
using Services.Interfaces;


namespace Services.UserService

{
    public class UserService : IUserService
    {
        private IHashService _hashService;
        private readonly IUserRepository _userRepository;
        public UserService(IHashService hashService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _hashService = hashService;
        }
        public async Task<CreateUserDTO> CreateUserService(CreateUserRequestDTO createUserRequest)
        {
            try
            {
                var emailAlreadyExists = await _userRepository.DataCompareEmailUserByMail(createUserRequest.mail_user);

                if (emailAlreadyExists != null) return new CreateUserDTO{ result = false, msg = "The email is already in use" };
                createUserRequest.password_user = _hashService.HashPasswordUser(createUserRequest.password_user);
                User newUser = await _userRepository.DataCreateUser(createUserRequest);
                return new CreateUserDTO { id_user = newUser.id_user, name_user = newUser.name_user, mail_user = newUser.mail_user, msg = "Ok", result = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error {ex.Message}");
                return new CreateUserDTO { result = false, msg = ex.Message };
            }
        }


        public async Task<GetUserByIdDTO> GetUserByIdProtectedService(GetUserByIdRequestDTO request)
        {
            try
            {

                User user = await _userRepository.DataGetUserByID(request.id_user);

                if (user == null) return new GetUserByIdDTO { msg = "User not found", result = false };

                if (!_hashService.VerifyPassword(request.password_user, user.password_user)) return new GetUserByIdDTO { msg = "Incorrect password", result = false };

                return new GetUserByIdDTO { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, msg = "OK", result = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to get a user by ID: {ex.Message}");
                return new GetUserByIdDTO { msg = ex.Message, result = false };
            }
        }
        public async Task<GetUserByIdDTO> GetUserByIdService(int id_user)
        {

            try
            {

                User user = await _userRepository.DataGetUserByID(id_user);
                if (user == null) return new GetUserByIdDTO { msg = "User not found", result = false };

                return new GetUserByIdDTO { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, msg = "OK", result = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to get a user by ID: {ex.Message}");
                return new GetUserByIdDTO { msg = ex.Message, result = false }; ;
            }
        }


        public async Task<int> UpdateUserByIdService(UpdateUserRequestDTO updateUserRequestDTO)
        {
            var rowsAffected = 0;
            try
            {


                // de no modificarse el campo nombre_user retornara 0, caso correcto retornara 1
                rowsAffected = await _userRepository.DataUpdateUserById(updateUserRequestDTO);
                if (rowsAffected == 0) return rowsAffected;

                return rowsAffected;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to update user by ID: {ex.Message}");
                return rowsAffected;
            }
        }
        public async Task<int> DeleteUserByIdService(int id)
        {
            var rowsAffected = 0;
            try
            {

                rowsAffected = await _userRepository.DataDeleteUserById(id);
                return rowsAffected;


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server Error {ex.Message}");
                return rowsAffected;
            }
        }
        //--------------------------auxiliary functions-------------------------------------
        public bool IsValidEmail(string mail)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(mail);
                return addr.Address == mail;
            }
            catch
            {
                return false;
            }
        }

    }
}
