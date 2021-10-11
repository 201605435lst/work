using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace SnAbp.PermissionManagement.MongoDB
{
    public class MongoPermissionGrantRepository : MongoDbRepository<IPermissionManagementMongoDbContext, PermissionGrant, Guid>, IPermissionGrantRepository
    {
        public MongoPermissionGrantRepository(IMongoDbContextProvider<IPermissionManagementMongoDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

        }

        public Task Delete(Expression<Func<PermissionGrant, bool>> condition)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<PermissionGrant> FindAsync(
            string name, 
            string providerName, 
            Guid providerGuid,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(s =>
                    s.Name == name &&
                    s.ProviderName == providerName &&
                    s.ProviderGuid == providerGuid,
                    GetCancellationToken(cancellationToken)
                );
        }

        public Task<PermissionGrant> FindAsync(string name, string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetGrantNameAsync(List<Guid> providerGuids, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<List<PermissionGrant>> GetListAsync(
            string providerName,
            Guid providerGuid,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(s =>
                    s.ProviderName == providerName &&
                    s.ProviderGuid == providerGuid
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<PermissionGrant>> GetListAsync(string providerName, string providerKey, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(s =>
                    s.ProviderName == providerName &&
                    s.ProviderKey == providerKey
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}