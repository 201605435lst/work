using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace SnAbp.Identity.MongoDB
{
    public class MongoIdentityRoleRepository : MongoDbRepository<ISnAbpIdentityMongoDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        public MongoIdentityRoleRepository(IMongoDbContextProvider<ISnAbpIdentityMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityRole>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                        x => x.Name.Contains(filter) ||
                        x.NormalizedName.Contains(filter))
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .As<IMongoQueryable<IdentityRole>>()
                .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetDefaultOnesAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().Where(r => r.IsDefault).ToListAsync(cancellationToken: GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                         x.NormalizedName.Contains(filter))
                .As<IMongoQueryable<IdentityRole>>()
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public Task<List<IdentityRole>> GetListAsync(List<Guid> limitGuids = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(List<Guid> limitGuids = null, string filter = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityRole>> GetListByOrganizationIdsAsync(List<Guid> organizationIds = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityRole>> GetListAsync(Guid organizationId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityRole>> GetListAsync(Guid? organizationId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityRole>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}