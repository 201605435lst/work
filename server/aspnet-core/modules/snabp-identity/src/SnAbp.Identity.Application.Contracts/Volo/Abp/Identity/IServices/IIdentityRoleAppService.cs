using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SnAbp.PermissionManagement;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Identity
{
    public interface IIdentityRoleAppService : IApplicationService
    {
        Task<ListResultDto<IdentityRoleDto>> GetAllListAsync();
        
        Task<PagedResultDto<IdentityRoleDto>> GetListAsync(IdentityRolePageDto input);
        Task<GetPermissionListResultDto> GetRolePermissionAsync([NotNull] string providerName, [NotNull] Guid providerGuid);

        Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input);

        Task<IdentityRoleDto> GetAsync(Guid id);

        Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input);

        Task DeleteAsync(Guid id);

        Task<bool> CheckDefaultRole();

        Task SetRolePermission(string providerName, Guid providerGuid, UpdatePermissionsDto input);
    }
}