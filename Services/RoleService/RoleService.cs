using AccesData.DTOs.BookDTOs;
using AccesData.DTOs.RoleDTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using AccesData.Repositories;
using Services.Interfaces;

namespace Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<CreateRoleDTO> CreateRoleService(CreateRoleRequest roleRequest)
        {
            try
            {
                var roleAlreadyExists = await _roleRepository.DataCompareNameRole(roleRequest.name_role);

                if (roleAlreadyExists != null) return new CreateRoleDTO { msg = "The role is already in use" };

                CreateRoleDTO newBook = await _roleRepository.DataCreateRole(roleRequest);

                if (newBook.msg == "error database") return new CreateRoleDTO { msg = "server error" };

                return new CreateRoleDTO { name_role = roleRequest.name_role, description_role = roleRequest.description_role, msg = "ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error {ex.Message}");
                return new CreateRoleDTO { msg = "server error" };
            }
        }

    }
}
