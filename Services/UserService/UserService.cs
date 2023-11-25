using AccesData.DTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using AccesData.Models;
using Services.Interfaces;


namespace Services.UserService

{
    public class UserService : IUserService
    {
        private IHashService _hashService;
        private readonly IUserRepository _userRepository;
        private IRoleRepository _roleRepository;
        public UserService(IHashService hashService, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _hashService = hashService;
            _roleRepository = roleRepository;
        }


        //crear un nuevo usuario con roles, los roles solo pueden coincidir con los registrados en la tabla "role"
        //Solo el usuario "Admin" va a tener acceso a este metodo
        public async Task<CreateUserWithRoleDTO> CreateUserWithRoleService(CreateUserWithRoleRequest createUserRequest)
        {
            try
            {
                var emailAlreadyExists = await _userRepository.DataCompareEmailUserByMail(createUserRequest.mail_user);
                var roleAlreadyExists = await _roleRepository.DataCompareNameRole(createUserRequest.role_user);

                if (roleAlreadyExists == null) return new CreateUserWithRoleDTO { msg = "The role does not exist" };

                if (emailAlreadyExists != null) return new CreateUserWithRoleDTO { msg = "The email is already in use" };

                createUserRequest.password_user = _hashService.HashPasswordUser(createUserRequest.password_user);
                CreateUserWithRoleDTO newUser = await _userRepository.DataCreateUserWithRole(createUserRequest);

                if (newUser.msg == "error database") return new CreateUserWithRoleDTO { msg = "server error" };
                return new CreateUserWithRoleDTO { id_user = newUser.id_user, name_user = newUser.name_user, mail_user = newUser.mail_user, role_user = roleAlreadyExists, msg = "Ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error {ex.Message}");
                return new CreateUserWithRoleDTO { msg = "server error" };
            }
        }

        public async Task<GetUserByIdDTO> GetUserByIdProtectedService(GetUserByIdRequest request)
        {
            try
            {

                User user = await _userRepository.DataGetUserByID(request.id_user);

                if (user == null) return new GetUserByIdDTO { msg = "User not found" };

                if (!_hashService.VerifyPassword(request.password_user, user.password_user)) return new GetUserByIdDTO { msg = "Incorrect password" };

                return new GetUserByIdDTO { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, msg = "OK" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to get a user by ID: {ex.Message}");
                return new GetUserByIdDTO { msg = "server error" };
            }
        }
        public async Task<GetUserByIdDTO> GetUserByIdService(int id_user)
        {

            try
            {

                User user = await _userRepository.DataGetUserByID(id_user);
                if (user == null) return new GetUserByIdDTO { msg = "User not found" };

                return new GetUserByIdDTO { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, msg = "OK" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to get a user by ID: {ex.Message}");
                return new GetUserByIdDTO { msg = "server error" }; ;
            }
        }


        public async Task<int> UpdateUserByIdService(UpdateUserRequest updateUserRequestDTO)
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
