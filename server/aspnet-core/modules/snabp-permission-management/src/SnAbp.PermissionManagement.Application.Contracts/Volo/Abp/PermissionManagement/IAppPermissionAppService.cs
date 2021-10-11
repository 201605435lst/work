using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Application.Services;

namespace SnAbp.PermissionManagement
{
    public interface IAppPermissionAppService : IApplicationService
    {
        Task<GetPermissionListResultDto> GetAsync([NotNull] string providerName, [NotNull] Guid providerGuid);
        Task UpdateAsync([NotNull] string providerName, [NotNull] Guid providerGuid, UpdatePermissionsDto input);
    }
}