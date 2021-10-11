using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using SnAbp.Identity.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp;

namespace SnAbp.Identity
{
    /// <summary>
    /// Performs domain logic for Organization Units.
    /// </summary>
    public class OrganizationManager : DomainService
    {
        protected IOrganizationRepository OrganizationRepository { get; }
        protected IStringLocalizer<IdentityResource> Localizer { get; }
        protected IIdentityRoleRepository IdentityRoleRepository { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public OrganizationManager(
            IOrganizationRepository organizationRepository,
            IStringLocalizer<IdentityResource> localizer,
            IIdentityRoleRepository identityRoleRepository,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            OrganizationRepository = organizationRepository;
            Localizer = localizer;
            IdentityRoleRepository = identityRoleRepository;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        //[UnitOfWork]
        public virtual async Task CreateAsync(Organization organization)
        {
            organization.Code = await GetNextChildCodeAsync(organization.ParentId);
            //判断是否根级组织机构，如果是，更改其上下级所有组织机构的isRoot属性为false
            if (organization.IsRoot)
            {
                var organizationRoot = OrganizationRepository.FirstOrDefault(x => x.IsRoot && x.Code.StartsWith(organization.Code));
                if (organizationRoot != null)
                {
                    organizationRoot.IsRoot = false;
                    await OrganizationRepository.UpdateAsync(organizationRoot);
                }
            }
            await ValidateOrganizationAsync(organization); // 验证组织机构是否存在
            await OrganizationRepository.InsertAsync(organization);
        }

        public virtual async Task UpdateAsync(Organization organization)
        {
            //await ValidateOrganizationAsync(organization);
            await OrganizationRepository.UpdateAsync(organization);
        }

        public virtual async Task UpdateRangesAsync(IEnumerable<Organization> organizations)
        {
            await OrganizationRepository.UpdateRanges(organizations);
        }

        public virtual async Task InsertRangesAsync(IEnumerable<Organization> organizations)
        {
            await OrganizationRepository.InsertRanges(organizations);
        }

        public virtual async Task<Organization> GetAsync(Guid id) => await OrganizationRepository.GetAsync(id);

        public virtual async Task<Organization> GetAsync(Expression<Func<Organization, bool>> expression) => await OrganizationRepository.GetAsync(expression);

        public Task<IQueryable<Organization>> Where(Expression<Func<Organization, bool>> func) => OrganizationRepository.Where(func);

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await OrganizationRepository.RemoveAllMembersAsync(child);
                await OrganizationRepository.RemoveAllRolesAsync(child);
                await OrganizationRepository.DeleteAsync(child);
            }

            var organization = await OrganizationRepository.GetAsync(id);

            await OrganizationRepository.RemoveAllMembersAsync(organization);
            await OrganizationRepository.RemoveAllRolesAsync(organization);
            await OrganizationRepository.DeleteAsync(id);
        }


        #region abp 自有方法

        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null)
            {
                return Organization.CalculateNextCode(lastChild.Code);
            }

            var parentCode = parentId != null
                ? await GetCodeOrDefaultAsync(parentId.Value)
                : null;

            return Organization.AppendCode(
                parentCode,
                Organization.CreateCode(1)
            );
        }

        public virtual async Task<Organization> GetLastChildOrNullAsync(Guid? parentId)
        {
            var children = await OrganizationRepository.GetChildrenAsync(parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var organization = await OrganizationRepository.GetAsync(id);
            if (organization.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of OU
            var oldCode = organization.Code;

            //Move OU
            organization.Code = await GetNextChildCodeAsync(parentId);
            organization.ParentId = parentId;

            // await ValidateOrganizationAsync(organization);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = Organization.AppendCode(organization.Code, Organization.GetRelativeCode(child.Code, oldCode));
            }
        }

        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var ou = await OrganizationRepository.GetAsync(id);
            return ou?.Code;
        }

        protected virtual async Task ValidateOrganizationAsync(Organization organization)
        {
            var siblings = (await FindChildrenAsync(organization.ParentId))
                .Where(ou => ou.Id != organization.Id)
                .ToList();

            if (siblings.Any(ou => ou.Name == organization.Name))
            {
                throw new UserFriendlyException($"同级已存在名称为:{organization.Name}的组织机构了");
            }
        }

        public async Task<List<Organization>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await OrganizationRepository.GetChildrenAsync(parentId, includeDetails: true);
            }

            if (!parentId.HasValue)
            {
                return await OrganizationRepository.GetListAsync(includeDetails: true);
            }

            var code = await GetCodeOrDefaultAsync(parentId.Value);

            return await OrganizationRepository.GetAllChildrenWithParentCodeAsync(code, parentId, includeDetails: true);
        }

        public virtual Task<bool> IsInOrganizationAsync(IdentityUser user, Organization ou)
        {
            return Task.FromResult(user.IsInOrganization(ou.Id));
        }

        public virtual async Task AddRoleToOrganizationAsync(Guid roleId, Guid ouId)
        {
            await AddRoleToOrganizationAsync(
                await IdentityRoleRepository.GetAsync(roleId),
                await OrganizationRepository.GetAsync(ouId, true)
                );
        }

        public virtual Task AddRoleToOrganizationAsync(IdentityRole role, Organization ou)
        {
            var currentRoles = ou.Roles;

            if (currentRoles.Any(r => r.OrganizationId == ou.Id && r.RoleId == role.Id))
            {
                return Task.FromResult(0);
            }
            ou.AddRole(role.Id);
            return Task.FromResult(0);
        }

        public virtual async Task RemoveRoleFromOrganizationAsync(Guid roleId, Guid ouId)
        {
            await RemoveRoleFromOrganizationAsync(
                await IdentityRoleRepository.GetAsync(roleId),
                await OrganizationRepository.GetAsync(ouId, true)
                );
        }

        public virtual Task RemoveRoleFromOrganizationAsync(IdentityRole role, Organization organization)
        {
            organization.RemoveRole(role.Id);
            return Task.FromResult(0);
        }

        public async Task<bool> Any()
        {
            return (await OrganizationRepository.Where(a => true)).Any();
        }

        public virtual async Task<List<Organization>> GetListAsync(List<Guid> ids, bool includeDetails = false) => await OrganizationRepository.GetListAsync(ids, true);

        #endregion
    }
}