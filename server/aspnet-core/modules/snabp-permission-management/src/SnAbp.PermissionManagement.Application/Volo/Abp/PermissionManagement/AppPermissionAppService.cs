using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Users;

namespace SnAbp.PermissionManagement
{
    [Authorize]
    public class AppPermissionAppService : ApplicationService, IAppPermissionAppService
    {
        protected PermissionManagementOptions Options { get; }

        protected IPermissionManager PermissionManager { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }

        public AppPermissionAppService(
            IPermissionManager permissionManager, 
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IOptions<PermissionManagementOptions> options)
        {
            Options = options.Value;
            PermissionManager = permissionManager;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
        }

        public virtual async Task<GetPermissionListResultDto> GetAsync(string providerName, Guid providerGuid)
        {
            await CheckProviderPolicy(providerName);

            var result = new GetPermissionListResultDto
            {
                // TODO Easten 改造疑问点
                EntityDisplayName = providerGuid.ToString(),
                Groups = new List<PermissionGroupDto>()
            };

            var multiTenancySide = CurrentTenant.GetMultiTenancySide();

            foreach (var group in PermissionDefinitionManager.GetGroups())
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName.Localize(StringLocalizerFactory),
                    Permissions = new List<PermissionGrantInfoDto>()
                };

                foreach (var permission in group.GetPermissionsWithChildren())
                {
                    if (!permission.IsEnabled)
                    {
                        continue;
                    }

                    if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
                    {
                        continue;
                    }

                    if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
                    {
                        continue;
                    }

                    var grantInfoDto = new PermissionGrantInfoDto
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName.Localize(StringLocalizerFactory),
                        ParentName = permission.Parent?.Name,
                        AllowedProviders = permission.Providers,
                        GrantedProviders = new List<ProviderInfoDto>()
                    };

                    

                    var grantInfo = await PermissionManager.GetAsync(permission.Name, providerName, providerGuid);

                    grantInfoDto.IsGranted = grantInfo.IsGranted;

                    foreach (var provider in grantInfo.Providers)
                    {
                        grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                        {
                            ProviderName = provider.Name,
                            ProviderGuid = provider.Guid,
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }

        public virtual async Task<GetPermissionListResultDto> GetPermissionAsync(string providerName, Guid providerGuid, List<Guid> roleIds = null)
        {
            await CheckProviderPolicy(providerName);

            var result = new GetPermissionListResultDto
            {
                // TODO Easten 改造疑问点
                EntityDisplayName = providerGuid.ToString(),
                Groups = new List<PermissionGroupDto>()
            };

            var multiTenancySide = CurrentTenant.GetMultiTenancySide();

            var currentUserRoleGrants = await PermissionGrantRepository.GetGrantNameAsync(roleIds);


            foreach (var group in PermissionDefinitionManager.GetGroups())
            {
                var groupDto = new PermissionGroupDto
                {
                    Name = group.Name,
                    DisplayName = group.DisplayName.Localize(StringLocalizerFactory),
                    Permissions = new List<PermissionGrantInfoDto>()
                };

                foreach (var permission in group.GetPermissionsWithChildren())
                {
                    if (!permission.IsEnabled)
                    {
                        continue;
                    }

                    if (permission.Providers.Any() && !permission.Providers.Contains(providerName))
                    {
                        continue;
                    }

                    if (!permission.MultiTenancySide.HasFlag(multiTenancySide))
                    {
                        continue;
                    }


                    // Easten 新增  权限处理

                    if (currentUserRoleGrants.Any() && !currentUserRoleGrants.Contains(permission.Name))
                    {
                        continue;
                    }

                    var grantInfoDto = new PermissionGrantInfoDto
                    {
                        Name = permission.Name,
                        DisplayName = permission.DisplayName.Localize(StringLocalizerFactory),
                        ParentName = permission.Parent?.Name,
                        AllowedProviders = permission.Providers,
                        GrantedProviders = new List<ProviderInfoDto>()
                    };



                    var grantInfo = await PermissionManager.GetAsync(permission.Name, providerName, providerGuid);

                    grantInfoDto.IsGranted = grantInfo.IsGranted;

                    foreach (var provider in grantInfo.Providers)
                    {
                        grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                        {
                            ProviderName = provider.Name,
                            ProviderGuid = provider.Guid,
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }


        public virtual async Task UpdateAsync(string providerName, Guid providerGuid, UpdatePermissionsDto input)
        {
            await CheckProviderPolicy(providerName);
            // 更新前先删除
            await PermissionGrantRepository.Delete(a => a.ProviderGuid == providerGuid);
            foreach (var permissionDto in input.Permissions)
            {
                await PermissionManager.SetAsync(permissionDto.Name, providerName, providerGuid, permissionDto.IsGranted);
            }
        }

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{policyName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
