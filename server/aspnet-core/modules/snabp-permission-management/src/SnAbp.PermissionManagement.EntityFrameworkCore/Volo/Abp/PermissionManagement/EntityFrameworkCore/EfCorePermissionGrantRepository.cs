using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.PermissionManagement.EntityFrameworkCore
{
    public class EfCorePermissionGrantRepository : EfCoreRepository<IPermissionManagementDbContext, PermissionGrant, Guid>,
        IPermissionGrantRepository
    {
        public EfCorePermissionGrantRepository(IDbContextProvider<IPermissionManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task Delete(Expression<Func<PermissionGrant, bool>> condition)
        {
            var list = DbSet.Where(condition);
            DbSet.RemoveRange(list);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<PermissionGrant> FindAsync(
            string name,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .FirstOrDefaultAsync(s =>
                    s.Name == name &&
                    s.ProviderName == providerName &&
                    s.ProviderKey == providerKey,
                    GetCancellationToken(cancellationToken)
                );
        }

        public async Task<PermissionGrant> FindAsync(string name, string providerName, Guid providerGuid, CancellationToken cancellationToken = default)
        {
            return await DbSet.FirstOrDefaultAsync(a =>
                a.Name == name &&
                a.ProviderGuid == providerGuid &&
                a.ProviderName == providerName,
                GetCancellationToken(cancellationToken)
            );
        }

        /// <summary>
        ///  获取指定权限提供者id集合内的所有权限的名字
        /// </summary>
        /// <param name="providerGuids"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<string>> GetGrantNameAsync(List<Guid> providerGuids, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(a => providerGuids.Contains(a.ProviderGuid)).Select(a => a.Name).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<PermissionGrant>> GetListAsync(
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(s =>
                    s.ProviderName == providerName &&
                    s.ProviderKey == providerKey
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<PermissionGrant>> GetListAsync(string providerName, Guid providerGuid, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(s =>
                    s.ProviderName == providerName &&
                    s.ProviderGuid == providerGuid
                ).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
