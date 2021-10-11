using JetBrains.Annotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using SnAbp.PermissionManagement;
using SnAbp.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace SnAbp.Identity
{
    [Authorize]
    public class AppRoleAppService : IdentityAppServiceBase, IIdentityRoleAppService
    {
        protected IdentityRoleManager RoleManager { get; }
        protected IdentityUserManager UserManager { get; }
        protected IOrganizationRepository OrganizationRepository { get; }
        protected IIdentityRoleRepository RoleRepository { get; }

        protected PermissionManagementOptions Options { get; }

        protected IPermissionManager PermissionManager { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }


        public AppRoleAppService(
            IdentityRoleManager roleManager,
            IOrganizationRepository organizationRepository,
            IdentityUserManager userManager,
            IPermissionManager permissionManager,
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IOptions<PermissionManagementOptions> options,
            IIdentityRoleRepository roleRepository)
        {
            RoleManager = roleManager;
            UserManager = userManager;
            RoleRepository = roleRepository;
            OrganizationRepository = organizationRepository;
            Options = options.Value;
            PermissionManager = permissionManager;
            PermissionGrantRepository = permissionGrantRepository;
            PermissionDefinitionManager = permissionDefinitionManager;
        }

        public virtual async Task<IdentityRoleDto> GetAsync(Guid id) =>
            ObjectMapper.Map<IdentityRole, IdentityRoleDto>(
                await RoleManager.GetByIdAsync(id)
            );



        public virtual async Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
        {
            var list = await RoleRepository.GetListAsync();
            return new ListResultDto<IdentityRoleDto>(
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list)
            );
        }

        public virtual async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(IdentityRolePageDto input)
        {
            // 获取角色信息，系统用户只能获取系统的角色
            // 普通用户获取当前选中组织机构的角色
            var limitRoleIds = new List<Guid>();
            var list = new List<IdentityRole>();
            if (input.OrganizationId.HasValue)
            {
                // 判断角色是否时公开状态，如果不公开，则角色只能在当前组织机构显示，公开则只能在其子集中显示。
                var organization = await OrganizationRepository.GetAsync(a => a.Id == input.OrganizationId);
                if (organization == null) return new PagedResultDto<IdentityRoleDto>();
                var parentCodeArray = organization?.Code.GetParentOrganizationCodeArray();
                if (parentCodeArray != null)
                {
                    var organizationIds =
                        input.Public
                        ?
                            (await OrganizationRepository.Where(a => parentCodeArray.Contains(a.Code)))
                            .Select(a => a.Id)
                            .ToList()
                        :
                            new List<Guid>() { input.OrganizationId.Value };

                    list = await RoleRepository.GetListByOrganizationIdsAsync(
                        organizationIds,
                        input.Sorting,
                        input.MaxResultCount,
                        input.SkipCount) ?? new List<IdentityRole>();
                }
                // 同时查找当前组织机构含有的没有被公开的角色
                var privateRoles = await RoleRepository.GetListAsync(organization.Id);
                list.AddRange(privateRoles);

                if (input.Public)
                {
                    // 查询系统角色中被公开的角色
                    var systemPublicRole = await RoleRepository.GetSystemRolesAsync();
                    list.AddRange(systemPublicRole);
                }

                list = list.Distinct().ToList();
            }
            else
            {
                // 分析：当组织机构的id为空时，系统用户需要返回系统角色。普通用户不需要
                var isSystemUser = await UserManager.isSystem(CurrentUser.Id.Value);
                if (isSystemUser)
                {
                    limitRoleIds = null;
                    list = input.IsAssignRoles
                        ? await RoleRepository.GetListAsync()
                        : await RoleRepository.GetListAsync(limitRoleIds, input.Sorting, input.MaxResultCount, input.SkipCount);
                }

            }
            // 获取选中的组织结构
            //var totalCount = await RoleRepository.GetCountAsync(limitRoleIds);
            var result = new PagedResultDto<IdentityRoleDto>(
                 list.Count,
                ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(input.IsAssignRoles
                ? list.Skip(input.SkipCount).Take(int.MaxValue).ToList()
                : list.Skip(input.SkipCount).Take(input.MaxResultCount).ToList())
                );
            foreach (var item in result.Items)
            {
                item.isDistributed = await PermissionManager.IsGranted("R", item.Id);
            }
            return result;
        }

        public async Task<GetPermissionListResultDto> GetRolePermissionAsync([NotNull] string providerName, [NotNull] Guid providerGuid)
        {
            // 过滤掉不在当前用户角色权限中的数据
            var user = await UserManager.GetByIdAsync(CurrentUser.Id.GetValueOrDefault());
            var userRoles = user.Roles.Select(a => a.RoleId).ToList();
            var isSystemUser = !user.Organizations.Any();  // 是否为系统用户
            var currentUserRoleGrants = await PermissionGrantRepository.GetGrantNameAsync(userRoles);
            // 获取角色对应的权限
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
                    // Easten 新增  权限处理 ,如果档期那用户是系统用户，就不要处理了
                    if (currentUserRoleGrants.Any() && !currentUserRoleGrants.Contains(permission.Name) && !isSystemUser)
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

        /// <summary>
        /// 检查是否已有默认角色
        /// </summary>
        /// <returns></returns>
        public Task<bool> CheckDefaultRole()
        {
            return Task.FromResult(RoleRepository.Any(a => a.IsDefault));
        }

        [Authorize(IdentityPermissions.Roles.Create)]
        public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
        {
            var role = new IdentityRole(
                GuidGenerator.Create(),
                input.Name,
                CurrentTenant.Id
            )
            {
                IsDefault = input.IsDefault,
                IsPublic = input.IsPublic
            };

            input.MapExtraPropertiesTo(role);


            // 角色名称格式进行修改，名称@GUID


            // 判断名称是否重复，统一组织结构下不能重名
            if (input.OrganizationId.HasValue)
            {
                // 判断这一组织机构是否存在相同的角色名称
                if (await RoleRepository.CheckSameRoleName(input.OrganizationId.Value, role.Name))
                {
                    throw new UserFriendlyException("同一组织机构下已存在该角色名称");
                }
                role.ChangeName($"{role.Name}@{role.Id}");
                await RoleRepository.InsertAsync(role);
            }
            else
            {
                if (await RoleRepository.CheckSameRoleName(role.Name))
                {
                    role.ChangeName($"{role.Name}@{role.Id}");
                    await RoleRepository.InsertAsync(role);
                }
                else
                {
                    throw new UserFriendlyException("已存在相同的角色名称");
                }

            }


            await CurrentUnitOfWork.SaveChangesAsync();

            // 添加组织机构
            if (input.OrganizationId.HasValue)
            {
                var organzation = await OrganizationRepository.GetAsync(a => a.Id == input.OrganizationId);
                organzation.AddRole(role.Id);
            }
            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        [Authorize(IdentityPermissions.Roles.Update)]
        public virtual async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
        {
            var role = await RoleManager.GetByIdAsync(id);
            role.ConcurrencyStamp = input.ConcurrencyStamp;

            (await RoleManager.SetRoleNameAsync(role, input.Name)).CheckErrors();

            role.IsDefault = input.IsDefault;
            role.IsPublic = input.IsPublic;

            input.MapExtraPropertiesTo(role);


            //更新角色名称时需要重新修改角色格式
            role.UpdateName();
            (await RoleManager.UpdateAsync(role)).CheckErrors();
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, IdentityRoleDto>(role);
        }

        [Authorize(IdentityPermissions.Roles.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            var role = await RoleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return;
            }

            (await RoleManager.DeleteAsync(role)).CheckErrors();
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

        /// <summary>
        /// 角色授权  easten 新增，满足恶意攻击的问题，授权前根据当前用户所有权限进行判断
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerGuid"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        //[Authorize(IdentityPermissions.Roles.Authorization)]
        public async Task SetRolePermission(string providerName, Guid providerGuid, UpdatePermissionsDto input)
        {
            await CheckProviderPolicy(providerName);
            var user = await UserManager.GetByIdAsync(CurrentUser.Id.GetValueOrDefault());
            var isSystemUser = !user.Organizations.Any();
            var userRoles = user.Roles.Select(a => a.RoleId).ToList();
            var currentUserRoleGrants = await PermissionGrantRepository.GetGrantNameAsync(userRoles);

            // 添加前删除
            await PermissionGrantRepository.Delete(a => a.ProviderGuid == providerGuid);
            foreach (var permissionDto in input.Permissions)
            {
                //  如果是系统用户，就不需要判断
                if (isSystemUser)
                {
                    await PermissionManager.SetAsync(permissionDto.Name, providerName, providerGuid, permissionDto.IsGranted);
                }
                else
                {
                    // 判断,
                    if (currentUserRoleGrants.Contains(permissionDto.Name))
                    {
                        await PermissionManager.SetAsync(permissionDto.Name, providerName, providerGuid, permissionDto.IsGranted);
                    }
                }
            }
        }
    }
}