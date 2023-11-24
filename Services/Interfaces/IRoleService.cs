using AccesData.DTOs.RoleDTOs;
using AccesData.InputsRequest;

namespace Services.Interfaces
{
    public interface IRoleService
    {
        Task<CreateRoleDTO> CreateRoleService(CreateRoleRequest roleRequest);
    }
}