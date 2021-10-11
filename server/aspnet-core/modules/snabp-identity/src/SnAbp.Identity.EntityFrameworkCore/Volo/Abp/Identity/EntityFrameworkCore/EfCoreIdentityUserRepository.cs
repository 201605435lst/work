using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

using Volo.Abp.Identity;

namespace SnAbp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityUserRepository : EfCoreRepository<IIdentityDbContext, IdentityUser, Guid>, IIdentityUserRepository
    {
        public EfCoreIdentityUserRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<IdentityUser> FindByNormalizedUserNameAsync(
            string normalizedUserName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var data = DbSet
                .IncludeDetails(includeDetails);
            if (data.Any()) return data.FirstOrDefault(a => a.NormalizedUserName == normalizedUserName);
            else return null;
            //return await 
            //    .FirstOrDefaultAsync(
            //        u => u.NormalizedUserName == normalizedUserName,
            //        GetCancellationToken(cancellationToken)
            //    );
        }

        public virtual async Task<List<string>> GetRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = from userRole in DbContext.Set<IdentityUserRltRole>()
                        join role in DbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role.Name;
            var organizationIds = DbContext.Set<IdentityUserRltOrganization>().Where(q => q.UserId == id).Select(q => q.OrganizationId).ToArray();
            if (DbContext.Set<OrganizationRltRole>().Any())
            {
                var organizationRoleIds = await (
               from ouRole in DbContext.Set<OrganizationRltRole>()
               join ou in DbContext.Set<Organization>() on ouRole.OrganizationId equals ou.Id
               where organizationIds.Contains(ouRole.OrganizationId)
               select ouRole.RoleId
               ).ToListAsync(GetCancellationToken(cancellationToken));
                var orgUnitRoleNameQuery = DbContext.Roles.Where(r => organizationRoleIds.Contains(r.Id)).Select(n => n.Name);
                query = query.Union(orgUnitRoleNameQuery);
            }
           
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<string>> GetRoleNamesInOrganizationAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = from userOu in DbContext.Set<IdentityUserRltOrganization>()
                        join roleOu in DbContext.Set<OrganizationRltRole>() on userOu.OrganizationId equals roleOu.OrganizationId
                        join ou in DbContext.Set<Organization>() on roleOu.OrganizationId equals ou.Id
                        join userOuRoles in DbContext.Roles on roleOu.RoleId equals userOuRoles.Id
                        where userOu.UserId == id
                        select userOuRoles.Name;

            var result = await query.ToListAsync(GetCancellationToken(cancellationToken));

            return result;
        }

        public virtual async Task<IdentityUser> FindByLoginAsync(
            string loginProvider,
            string providerKey,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<IdentityUser> FindByNormalizedEmailAsync(
            string normalizedEmail,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var role = await DbContext.Roles
                .Where(x => x.NormalizedName == normalizedRoleName)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));

            if (role == null)
            {
                return new List<IdentityUser>();
            }

            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetListAsync(
            Guid? organizationId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var userids = DbContext.Set<IdentityUserRltOrganization>().Where(a => a.OrganizationId == organizationId)?.Select(a => a.UserId).ToList();
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.Name != null && u.Name.Contains(filter)) ||
                        (u.Surname != null && u.Surname.Contains(filter)) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                )
                .WhereIf(condition: organizationId != null, predicate: a => userids.Contains(a.Id))
                .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                // .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
        public async Task<List<IdentityUser>> GetUserListAsync(string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            Guid? organizationId = null,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            // 获取当前排除组织机构的子集的所有用户
            var organization = DbContext.Set<Organization>().IncludeDetails(true).FirstOrDefault(args => args.Id == organizationId);
            if (organization != null && organization.Children.Any())
            {
                var orgIds = organization.Children.Select(b => b.Id).ToList();
                var userids = DbContext.Set<IdentityUserRltOrganization>().Where(a => orgIds.Contains(a.OrganizationId)).Select(b => b.UserId).ToList();
                return await DbSet.IncludeDetails(true)
                    .WhereIf(!filter.IsNullOrWhiteSpace(), a => a.UserName.Contains(filter) || a.Email.Contains(filter) || (a.Name != null && a.Name.Contains(filter)) || (a.Surname != null && a.Surname.Contains(filter)) || (a.PhoneNumber != null && a.PhoneNumber.Contains(filter)))
                    .Where(a => userids.Contains(a.Id))
                    .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                    //.PageBy(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                return null;
            }
        }
        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from userRole in DbContext.Set<IdentityUserRltRole>()
                        join role in DbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role;

            //TODO: Needs improvement
            var userOrganizationsQuery = from userOrg in DbContext.Set<IdentityUserRltOrganization>()
                                         join ou in DbContext.Organization.IncludeDetails(includeDetails) on userOrg.OrganizationId equals ou.Id
                                         where userOrg.UserId == id
                                         select ou;

            var orgUserRoleQuery = DbContext.Set<OrganizationRltRole>()
                .Where(q => userOrganizationsQuery
                .Select(t => t.Id)
                .Contains(q.OrganizationId))
                .Select(t => t.RoleId);

            var orgRoles = DbContext.Roles.Where(q => orgUserRoleQuery.Contains(q.Id));
            var resultQuery = query.Union(orgRoles);

            return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
        }


        public Task<List<IdentityRole>> GetRolesAsync(Guid id, bool includeDetails = false)
        {
            var query = from userRole in DbContext.Set<IdentityUserRltRole>()
                        join role in DbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role;

            return query.ToListAsync();
        }





        public virtual async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await this.WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.Name != null && u.Name.Contains(filter)) ||
                        (u.Surname != null && u.Surname.Contains(filter)) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                )
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Organization>> GetOrganizationsAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from userOU in DbContext.Set<IdentityUserRltOrganization>()
                        join ou in DbContext.Organization.IncludeDetails(includeDetails) on userOU.OrganizationId equals ou.Id
                        where userOU.UserId == id
                        select ou;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationAsync(
            Guid organizationId,
            CancellationToken cancellationToken = default
            )
        {
            var query = from userOu in DbContext.Set<IdentityUserRltOrganization>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where userOu.OrganizationId == organizationId
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationIds,
            CancellationToken cancellationToken = default
            )
        {
            var query = from userOu in DbContext.Set<IdentityUserRltOrganization>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where organizationIds.Contains(userOu.OrganizationId)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetUsersInOrganizationWithChildrenAsync(
            string code,
            CancellationToken cancellationToken = default
            )
        {
            var query = from userOu in DbContext.Set<IdentityUserRltOrganization>()
                        join user in DbSet on userOu.UserId equals user.Id
                        join ou in DbContext.Set<Organization>() on userOu.OrganizationId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<IdentityUser> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public async Task<IdentityUser> FindByUserNameAsync([NotNull] string userName, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails().FirstOrDefaultAsync(a => a.UserName == userName, cancellationToken: cancellationToken);
        }

        public async Task<List<IdentityUser>> GetUserListAsync(Expression<Func<IdentityUser, bool>> func, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(func).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task DeleteAsync(Expression<Func<IdentityUser, bool>> func, CancellationToken cancellationToken = default)
        {
            var users = await DbSet.Where(func).ToListAsync(GetCancellationToken(cancellationToken));
            foreach (var item in users)
            {
                DbSet.Remove(item);
            }
            await DbContext.SaveChangesAsync(GetCancellationToken(cancellationToken));
        }

        public async Task DeleteAsync(Expression<Func<IdentityUserRltOrganization, bool>> func, CancellationToken cancellationToken = default)
        {
            var list = await DbContext.Set<IdentityUserRltOrganization>().Where(func).ToListAsync(GetCancellationToken(cancellationToken));
            foreach (var item in list)
            {
                DbContext.Set<IdentityUserRltOrganization>().Remove(item);
            }
            await DbContext.SaveChangesAsync();
        }

        public async Task CreateUserOrganization(IdentityUserRltOrganization model, CancellationToken cancellationToken = default)
        {
            await DbContext.Set<IdentityUserRltOrganization>().AddAsync(model, GetCancellationToken(cancellationToken));

            await DbContext.SaveChangesAsync();
        }

        public async Task<List<IdentityUser>> GetUserList(Expression<Func<IdentityUser, bool>> func, bool includeDetail = false)
        {
            return includeDetail ? await DbSet.Include(a=>a.Organizations).ThenInclude(a=>a.Organization).Where(func).ToListAsync(): await DbSet.Where(func).ToListAsync();
        }

        public async Task<bool> CreateUserAndProject(IdentityUserRltProject userRltProject)
        {
            await DbContext.Set<IdentityUserRltProject>().AddAsync(userRltProject, GetCancellationToken(default));
            return true;
        }
    }
}