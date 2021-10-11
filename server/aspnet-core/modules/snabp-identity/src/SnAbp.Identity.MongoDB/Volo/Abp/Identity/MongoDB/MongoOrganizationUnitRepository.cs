using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace SnAbp.Identity.MongoDB
{
    public class MongoOrganizationRepository
        : MongoDbRepository<ISnAbpIdentityMongoDbContext, Organization, Guid>,
        IOrganizationRepository
    {
        public MongoOrganizationRepository(
            IMongoDbContextProvider<ISnAbpIdentityMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<Organization>> GetChildrenAsync(
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ou => ou.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Organization>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Organization>> GetListAsync(
            IEnumerable<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .Where(t => ids.Contains(t.Id))
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<Organization>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .OrderBy(sorting ?? nameof(Organization.Name))
                    .As<IMongoQueryable<Organization>>()
                    .PageBy<Organization, IMongoQueryable<Organization>>(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Organization> GetAsync(
            string displayName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(
                    ou => ou.Name == displayName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Organization organization,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var roleIds = organization.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id))
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .As<IMongoQueryable<IdentityRole>>()
                .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<int> GetRolesCountAsync(
            Organization organization,
            CancellationToken cancellationToken = default)
        {
            var roleIds = organization.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id))
                .As<IMongoQueryable<IdentityRole>>()
                .CountAsync(cancellationToken);
        }

        public virtual async Task<List<IdentityUser>> GetMembersAsync(
            Organization organization,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = CreateGetMembersFilteredQuery(organization, filter);

            return await query
                .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                .As<IMongoQueryable<IdentityUser>>()
                .PageBy<IdentityUser, IMongoQueryable<IdentityUser>>(skipCount, maxResultCount)
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

        public virtual Task RemoveAllRolesAsync(Organization organization, CancellationToken cancellationToken = default)
        {
            organization.Roles.Clear();
            return Task.FromResult(0);
        }

        public virtual async Task RemoveAllMembersAsync(Organization organization, CancellationToken cancellationToken = default)
        {
            var users = await DbContext.Users.AsQueryable()
                .Where(u => u.Organizations.Any(uou => uou.OrganizationId == organization.Id))
                .As<IMongoQueryable<IdentityUser>>()
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var user in users)
            {
                user.RemoveOrganization(organization.Id);
                DbContext.Users.ReplaceOne(u => u.Id == user.Id, user);
            }
        }

        protected virtual IMongoQueryable<IdentityUser> CreateGetMembersFilteredQuery(Organization organization, string filter = null)
        {
            return DbContext.Users.AsQueryable()
                .Where(u => u.Organizations.Any(uou => uou.OrganizationId == organization.Id))
                .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                );
        }

        public Task<IQueryable<Organization>> Where(Expression<Func<Organization, bool>> func)
        {
            // TODO 后去按需求进行实现
            throw new NotImplementedException();
        }

        public Task<Organization> GetAsync(Expression<Func<Organization, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRanges(IEnumerable<Organization> organizations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task InsertRanges(IEnumerable<Organization> organizations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Organization>> GetListAsync(string KeyWords, List<Guid> ids, Guid parentId, string startWithCode)
        {
            throw new NotImplementedException();
        }
    }
}