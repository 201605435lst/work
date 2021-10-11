using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Settings;
using SnAbp.Identity.Settings;
using Volo.Abp;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using SnAbp.PermissionManagement;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Volo.Abp.Identity.Repository;

namespace SnAbp.Identity
{
    public class IdentityUserManager : UserManager<IdentityUser>, IDomainService
    {
        protected IIdentityRoleRepository _roleRepository { get; }
        protected IIdentityUserRoleRepository _userRoleRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected IOrganizationRepository OrganizationRepository { get; }
        protected IIdentityUserRltOrganizationRepository IdentityUserRltOrganizationRepository;
        protected ISettingProvider SettingProvider { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        protected override CancellationToken CancellationToken => CancellationTokenProvider.Token;

        private readonly IGuidGenerator _generator;
        private readonly IOrganizationRltRoleRepository _organizationRltRoleRepository;
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        public IdentityUserManager(
            IdentityUserStore store,
            IIdentityUserRltOrganizationRepository identityUserRltOrganizationRepository,
            IIdentityUserRoleRepository userRoleRepository,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher,
            IEnumerable<IUserValidator<IdentityUser>> userValidators,
            IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<IdentityUserManager> logger,
            ICancellationTokenProvider cancellationTokenProvider,
            IOrganizationRepository organizationRepository,
            IOrganizationRltRoleRepository organizationRltRoleRepository,
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator generator,
            ISettingProvider settingProvider)
            : base(
                  store,
                  optionsAccessor,
                  passwordHasher,
                  userValidators,
                  passwordValidators,
                  keyNormalizer,
                  errors,
                  services,
                  logger)
        {
            IdentityUserRltOrganizationRepository = identityUserRltOrganizationRepository;
            OrganizationRepository = organizationRepository;
            SettingProvider = settingProvider;
            _roleRepository = roleRepository;
            UserRepository = userRepository;
            CancellationTokenProvider = cancellationTokenProvider;
            _userRoleRepository = userRoleRepository;
            _generator = generator;
            _organizationRltRoleRepository = organizationRltRoleRepository;
            _permissionGrantRepository = permissionGrantRepository;
        }

        public virtual async Task<IdentityUser> GetByIdAsync(Guid id)
        {
            var user = await Store.FindByIdAsync(id.ToString(), CancellationToken);
            if (user == null)
            {
                throw new EntityNotFoundException(typeof(IdentityUser), id);
            }

            return user;
        }

        public virtual async Task<IdentityResult> SetRolesAsync1([NotNull] IdentityUser user, [NotNull] IEnumerable<string> roleNames)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(roleNames, nameof(roleNames));

            var currentRoleNames = await GetRolesAsync(user);

            var result = await RemoveFromRolesAsync(user, currentRoleNames.Except(roleNames).Distinct());
            if (!result.Succeeded)
            {
                return result;
            }

            result = await AddToRolesAsync(user, roleNames.Except(currentRoleNames).Distinct());
            if (!result.Succeeded)
            {
                return result;
            }

            return IdentityResult.Success;
        }

        /// <summary>
        /// 方法扩展，设置用户的角色 Easten新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> SetRolesAsync([NotNull] IdentityUser user, [NotNull] IEnumerable<Guid> roleIds)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(roleIds, nameof(roleIds));

            // 获取当前用户的角色ids
            IEnumerable<Guid> currentRoleIds = (await _userRoleRepository.GetRoleIdsByUser(user.Id))?.ToList();

            if (currentRoleIds.Any())
            {
                var result = await RemoveFromRolesAsync(user, currentRoleIds.Except(roleIds).Distinct());
                if (!result.Succeeded)
                {
                    return result;
                }

                // 添加档期的角色
                result = await AddToRolesAsync(user, currentRoleIds.Except(roleIds).Distinct());
                if (!result.Succeeded)
                {
                    return result;
                }

            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// 移除当前用户的角色信息 Easten 新增
        /// </summary>
        /// <param name="user"></param>
        /// <param name="guids"></param>
        /// <returns></returns>
        private async Task<IdentityResult> RemoveFromRolesAsync(IdentityUser user, IEnumerable<Guid> guids)
        {
            try
            {
                await _userRoleRepository.DeleteAsync(a => a.UserId == user.Id && guids.Contains(a.RoleId), CancellationToken);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }

        /// <summary>
        /// 给用户添加角色信息 Easten 修改
        /// </summary>
        /// <param name="user"></param>
        /// <param name="guids"></param>
        /// <returns></returns>
        private async Task<IdentityResult> AddToRolesAsync(IdentityUser user, IEnumerable<Guid> guids)
        {
            try
            {
                foreach (var guid in guids)
                {
                    var model = new IdentityUserRltRole(user.Id, guid, null);
                    await _userRoleRepository.InsertAsync(model, cancellationToken: CancellationToken);
                }
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
        }


        public virtual async Task<bool> IsInOrganizationAsync(Guid userId, Guid ouId)
        {
            var user = await UserRepository.GetAsync(userId, cancellationToken: CancellationToken);
            return user.IsInOrganization(ouId);
        }

        public virtual async Task<bool> IsInOrganizationAsync(IdentityUser user, Organization ou)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Organizations, CancellationTokenProvider.Token);
            return user.IsInOrganization(ou.Id);
        }

        public virtual async Task AddToOrganizationAsync(Guid userId, Guid ouId)
        {
            await AddToOrganizationAsync(
                await UserRepository.GetAsync(userId, cancellationToken: CancellationToken),
                await OrganizationRepository.GetAsync(ouId, cancellationToken: CancellationToken)
                );
        }

        public virtual async Task AddToOrganizationAsync(IdentityUser user, Organization ou)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Organizations, CancellationTokenProvider.Token);

            if (user.Organizations.Any(cou => cou.OrganizationId == ou.Id))
            {
                return;
            }

            await CheckMaxUserOrganizationMembershipCountAsync(user.Organizations.Count + 1);

            user.AddOrganization(ou.Id);
        }

        public virtual async Task RemoveFromOrganizationAsync(Guid userId, Guid ouId)
        {
            var user = await UserRepository.GetAsync(userId, cancellationToken: CancellationToken);
            user.RemoveOrganization(ouId);
        }

        public virtual async Task RemoveFromOrganizationAsync(IdentityUser user, Organization ou)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Organizations, CancellationTokenProvider.Token);

            user.RemoveOrganization(ou.Id);
        }

        public virtual async Task SetOrganizationsAsync(Guid userId, params Guid[] organizationIds)
        {
            await SetOrganizationsAsync(
                await UserRepository.GetAsync(userId, cancellationToken: CancellationToken),
                organizationIds
            );
        }

        public virtual async Task SetOrganizationsAsync(IdentityUser user, params Guid[] organizationIds)
        {
            Check.NotNull(user, nameof(user));
            Check.NotNull(organizationIds, nameof(organizationIds));

            await CheckMaxUserOrganizationMembershipCountAsync(organizationIds.Length);

            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Organizations, CancellationTokenProvider.Token);

            //Remove from removed OUs
            foreach (var ouId in user.Organizations.Select(uou => uou.OrganizationId).ToArray())
            {
                if (!organizationIds.Contains(ouId))
                {
                    user.RemoveOrganization(ouId);
                }
            }

            //Add to added OUs
            foreach (var organizationId in organizationIds)
            {
                if (!user.IsInOrganization(organizationId))
                {
                    user.AddOrganization(organizationId);
                }
            }
        }

        private async Task CheckMaxUserOrganizationMembershipCountAsync(int requestedCount)
        {
            var maxCount = await SettingProvider.GetAsync<int>(IdentitySettingNames.Organization.MaxUserMembershipCount);
            if (requestedCount > maxCount)
            {
                throw new BusinessException(IdentityErrorCodes.MaxAllowedOuMembership)
                    .WithData("MaxUserMembershipCount", maxCount);
            }
        }

        [UnitOfWork]
        public virtual async Task<List<Organization>> GetOrganizationsAsync(IdentityUser user, bool includeDetails = false)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Organizations, CancellationTokenProvider.Token);

            return await OrganizationRepository.GetListAsync(
                user.Organizations.Select(t => t.OrganizationId),
                includeDetails,
                cancellationToken: CancellationToken
            );
        }

        [UnitOfWork]
        public virtual async Task<List<Organization>> GetOrganizationsAsync(Guid? userGuid, bool includeDetails = false) =>
            userGuid == null ? null : await UserRepository.GetOrganizationsAsync(userGuid.GetValueOrDefault(), includeDetails, cancellationToken: CancellationToken);

        [UnitOfWork]
        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationAsync(
            Organization organization,
            bool includeChildren = false)
        {
            if (includeChildren)
            {
                return await UserRepository
                    .GetUsersInOrganizationWithChildrenAsync(organization.Code, CancellationToken);
            }
            else
            {
                return await UserRepository
                    .GetUsersInOrganizationAsync(organization.Id, CancellationToken);
            }
        }
        [UnitOfWork]
        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationAsync(Guid organizationGuid) =>
            await UserRepository
                .GetUsersInOrganizationAsync(organizationGuid, CancellationToken);

        public virtual async Task<IdentityResult> AddDefaultRolesAsync([NotNull] IdentityUser user)
        {
            await UserRepository.EnsureCollectionLoadedAsync(user, u => u.Roles, CancellationToken);

            foreach (var role in await _roleRepository.GetDefaultOnesAsync(cancellationToken: CancellationToken))
            {
                if (!user.IsInRole(role.Id))
                {
                    user.AddRole(role.Id);
                }
            }

            return await UpdateUserAsync(user);
        }

        /// <summary>
        /// 获取根据用户id获取成员
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Member>> GetUserMembers(Guid userId)
        {
            var organizations = await UserRepository.GetOrganizationsAsync(userId, cancellationToken: CancellationToken);
            var roles = await UserRepository.GetRolesAsync(userId, false);

            var members = organizations.Select(organization => new Member { Type = MemberType.Organization, Name = organization.Name, Id = organization.Id }).ToList();
            members.AddRange(roles.Select(role => new Member { Type = MemberType.Role, Id = role.Id }));

            members.Add(new Member
            {
                Id = userId,
                Type = MemberType.User
            });

            return members;
        }

        public async Task<IdentityUser> GetUserByUserName(string userName) => await UserRepository.FindByUserNameAsync(userName, true, CancellationToken);

        /// <summary>
        /// 获取用户系统角色及当前组织机构角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserPermissions(Guid userId, Guid? organizationId)
        {
            // 如果有组织机构，则只返回当前组织机构的权限；
            // 否则返回非组织机构的权限
            // 获取用户所有角色
            var members = await GetUserMembers(userId);
            var allRoleIds = members.Where(x => x.Type == MemberType.Role).Select(x => x.Id).ToList();
            var allOrganizationIds = members.Where(x => x.Type == MemberType.Organization).Select(x => x.Id).ToList();
            var permissions = new List<string>();

            var roleIds = new List<Guid>();
            // 有组织机构，并且当前用户为组织列表的一员，返回当前登录的组织机构的角色Id
            if (organizationId != null && members.Select(x => x.Id).ToList().Contains(organizationId.Value))
            {
                // 原代码，问题：如果是父级共享角色，则取不到权限
                //roleIds = _organizationRltRoleRepository
                //    .Where(x => x.OrganizationId == organizationId && allRoleIds.Contains(x.RoleId))
                //    .Select(x => x.RoleId)
                //    .ToList();

                // bug修复：2020年11月4日14:04:43（lst）
                //分析：当前成员属于组织机构，需考虑父级的共享角色和当前组织机构的角色
                //1、获取当前用户的组织机构
                var currentOrganization = OrganizationRepository.FirstOrDefault(x => x.Id == organizationId);
                if (currentOrganization != null)
                {
                    var roleList = new List<Guid>();
                    //2、获取当前用户所在组织机构的code
                    var code = currentOrganization.Code;
                    //3、根据code获取当前用户的父级组织机构id
                    var organizationIds = OrganizationRepository.WhereIf(code != null, x => code.StartsWith(x.Code) && x.Code != code).Select(x => x.Id).ToList();
                    //4、获取父级的所有共享角色()
                    //4.1获取父级的所有角色id
                    var organizationRltRoles = _organizationRltRoleRepository.Where(x => organizationIds.Contains(x.OrganizationId));
                    var organizationRltRolesIds = organizationRltRoles.Select(x => x.RoleId).ToList();
                    //4.2 过滤得到父级的共享角色
                    var organizationRltRolesPublicIds = _roleRepository.Where(x => organizationRltRolesIds.Contains(x.Id) && x.IsPublic);
                    roleList.AddRange(organizationRltRolesIds);
                    //5、获取系统的所有共享角色
                    // 5.1、获取组织机构的角色
                    var orgRoleId = _organizationRltRoleRepository.Select(x => x.RoleId);
                    // 5.2、获取系统公开角色
                    var systermPubRole = _roleRepository.Where(x => !orgRoleId.Contains(x.Id) && x.IsPublic);
                    var systermPubRoleIds = systermPubRole.Select(x => x.Id).ToList();
                    // 6、获取系统和父级组织机构的共享角色
                    roleList.AddRange(systermPubRoleIds);
                    // 7、得到当前组织机构的角色
                    var currentOrganizationRolesId = _organizationRltRoleRepository
                        .Where(x => x.OrganizationId == organizationId).Select(x => x.RoleId).ToList();
                    //得到当前用户   可以   拥有的所有角色
                    roleList.AddRange(currentOrganizationRolesId);
                    // 8、得到当前用户分配的所有角色
                    var currentUserRole = _userRoleRepository.Where(x => x.UserId == userId);
                    // 9、得到当前用户在当前组织机构里面分配的所有角色
                    roleIds = currentUserRole.Where(x => roleList.Contains(x.RoleId)).Select(x => x.RoleId).ToList();
                }
            }
            else
            {
                // 返回该用户所有组织机构的角色Id
                var allOrganizationRoleIds = _organizationRltRoleRepository
                    .Where(x => allOrganizationIds
                    .Contains(x.OrganizationId))
                    .Select(x => x.RoleId)
                    .ToList();
                //该用户同时为系统用户
                var systemRoleIds = _roleRepository.Where(x => allRoleIds.Contains(x.Id)).Select(x => x.Id).ToList();
                //var allRolesIds = new List<Guid>();
                roleIds.AddRange(allOrganizationRoleIds);
                roleIds.AddRange(systemRoleIds);

                //roleIds = UserRoleRepository
                //    .Where(x => x.UserId == userId && !roleIds.Contains(x.RoleId))
                //    .Select(x => x.RoleId)
                //    .ToList();
            }

            var roles = _roleRepository.Where(x => roleIds.Contains(x.Id)).ToList();
            var roleIdsa = roles.Select(x => x.Id).ToList();

            permissions = _permissionGrantRepository
                .Where(x => x.ProviderName == "R" && roleIdsa.Contains(x.ProviderGuid))
                .Select(x => x.Name)
                .ToList();

            var addPermissions = new List<string>();

            foreach (var item in permissions)
            {
                var top = item.Split('.').FirstOrDefault();
                addPermissions.AddIfNotContains(top);
            }

            permissions.AddRange(addPermissions);

            return permissions.OrderBy(x => x).ToList();
        }

        public async Task<List<IdentityUser>> GetUserListAsync(Expression<Func<IdentityUser, bool>> func)
        {
            return await UserRepository.GetUserListAsync(func);
        }

        /// <summary>
        /// 判断当前用户是否为系统用户（判断依据为当前用户的角色至少包含一个非默认角色且非组织机构角色)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> isSystem(Guid userId)
        {
            // 分析：当前用户是否为系统用户
            /*
             1、情况1:当前用户只属于某一组织机构；不是系统角色
             2、情况2：当前用户不属于任何一组织机构；是系统角色
             */
            // 1、获取属于组织机构的用户
            var UserRltOrganizations = IdentityUserRltOrganizationRepository.ToList();
            return UserRltOrganizations.Find(x => x.UserId == userId) == null;
            //var members = await GetUserMembers(userId);
            //var rolesIds = members.Where(x => x.Type == MemberType.Role).Select(x => x.Id).ToList();

            //// 当前用户的所有角色id
            //var userRltroleIds = _userRoleRepository.Where(x => rolesIds.Contains(x.RoleId)).Select(x => x.RoleId).ToList();
            //// 当前用户的所有角色
            //var roles = _roleRepository.Where(x => userRltroleIds.Contains(x.Id)).ToList();

            //var organizationRoles = _organizationRltRoleRepository.Where(x => userRltroleIds.Contains(x.RoleId)).ToList();

            //// 判断依据为当前用户的角色至少包含一个非默认角色且非组织机构角色
            //return roles.Find(x => !organizationRoles.Select(x => x.RoleId).Contains(x.Id) && !x.IsDefault && !x.IsPublic) != null;
        }

        public async Task DeleteAsync(Expression<Func<IdentityUser, bool>> func)
        {
            await UserRepository.DeleteAsync(func);
        }
        public async Task DeleteUserOrganizationAsync(Expression<Func<IdentityUserRltOrganization, bool>> func)
        {
            await UserRepository.DeleteAsync(func);
        }
        public async Task CreateUserOrganizationAsync(IdentityUserRltOrganization model)
        {
            await UserRepository.CreateUserOrganization(model);
        }

        public async Task<bool> CreateUserProject(IdentityUserRltProject model)
        {
            return await UserRepository.CreateUserAndProject(model);
        }
    }
}