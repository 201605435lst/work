using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;

namespace SnAbp.PermissionManagement
{
    [RemoteService(Name = PermissionManagementRemoteServiceConsts.RemoteServiceName)]
    [ControllerName("AppPermission")]
    [Area("permissionManagement")]
    [Route("api/permission-management/permissions")]
    public class PermissionsController : AbpController, IAppPermissionAppService
    {
        protected IAppPermissionAppService PermissionAppService { get; }

        public PermissionsController(IAppPermissionAppService permissionAppService)
        {
            PermissionAppService = permissionAppService;
        }

        [HttpGet]
        public virtual Task<GetPermissionListResultDto> GetAsync(string providerName, Guid providerGuid)
        {
            return PermissionAppService.GetAsync(providerName, providerGuid);
        }

        [HttpPut]
        public virtual Task UpdateAsync(string providerName, Guid providerGuid, UpdatePermissionsDto input)
        {
            return PermissionAppService.UpdateAsync(providerName, providerGuid, input);
        }


    }
}
