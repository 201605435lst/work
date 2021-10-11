using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SnAbp.Identity.EntityFrameworkCore
{
    public class OrganizationRepository : EfCoreRepository<IIdentityDbContext, Organization, Guid>, IOrganizationRepository
    {
        public OrganizationRepository(
            IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Organization>> GetChildrenAsync(
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(x => x.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Organization>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Organization>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .OrderBy(sorting ?? nameof(Organization.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
        public virtual async Task<List<Organization>> GetListAsync(
            IEnumerable<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Organization> GetAsync(
            string displayName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .FirstOrDefaultAsync(
                    ou => ou.Name == displayName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public virtual async Task<Organization> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(a => a.Id == id, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Organization> GetAsync(Expression<Func<Organization, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(true).FirstOrDefaultAsync(expression, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Organization organization,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from organizationRole in DbContext.Set<OrganizationRltRole>()
                        join role in DbContext.Roles.IncludeDetails(includeDetails) on organizationRole.RoleId equals role.Id
                        where organizationRole.OrganizationId == organization.Id
                        select role;
            query = query
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetRolesCountAsync(
            Organization organization,
            CancellationToken cancellationToken = default)
        {
            var query = from organizationRole in DbContext.Set<OrganizationRltRole>()
                        join role in DbContext.Roles on organizationRole.RoleId equals role.Id
                        where organizationRole.OrganizationId == organization.Id
                        select role;

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityUser>> GetMembersAsync(
            Organization organization,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
            )
        {
            var query = CreateGetMembersFilteredQuery(organization, filter);

            return await query.IncludeDetails(includeDetails).OrderBy(sorting ?? nameof(IdentityUser.UserName))
                        .PageBy(skipCount, maxResultCount)
                        .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetMembersCountAsync(
            Organization organization,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = CreateGetMembersFilteredQuery(organization, filter);

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<Organization> WithDetails() => GetQueryable().IncludeDetails();

        public virtual Task RemoveAllRolesAsync(
            Organization organization,
            CancellationToken cancellationToken = default)
        {
            organization.Roles.Clear();
            return Task.CompletedTask;
        }

        public virtual async Task RemoveAllMembersAsync(
            Organization organization,
            CancellationToken cancellationToken = default)
        {
            var ouMembersQuery = await DbContext.Set<IdentityUserRltOrganization>()
                .Where(q => q.OrganizationId == organization.Id)
                .ToListAsync(GetCancellationToken(cancellationToken));

            DbContext.Set<IdentityUserRltOrganization>().RemoveRange(ouMembersQuery);
        }

        protected virtual IQueryable<IdentityUser> CreateGetMembersFilteredQuery(Organization organization, string filter = null)
        {
            var query = from userOu in DbContext.Set<IdentityUserRltOrganization>()
                        join user in DbContext.Users on userOu.UserId equals user.Id
                        where userOu.OrganizationId == organization.Id
                        select user;

            if (!filter.IsNullOrWhiteSpace())
            {
                query = query.Where(u =>
                    u.UserName.Contains(filter) ||
                    u.Email.Contains(filter) ||
                    (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                );
            }

            return query;
        }

        public Task<IQueryable<Organization>> Where(Expression<Func<Organization, bool>> func) => Task.FromResult(DbSet.IncludeDetails(true).Where(func));

        public async Task UpdateRanges(IEnumerable<Organization> organizations, CancellationToken cancellationToken = default)
        {
            DbSet.UpdateRange(organizations);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task InsertRanges(IEnumerable<Organization> organizations, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(organizations, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}