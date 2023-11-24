using AccesData.DTOs.RoleDTOs;
using AccesData.InputsRequest;

namespace AccesData.Interfaces
{
    public interface IRoleRepository
    {
        Task<CreateRoleDTO> DataCreateRole(CreateRoleRequest roleRequest);
        Task<string> DataCompareNameRole(string nameRole);
    }
}