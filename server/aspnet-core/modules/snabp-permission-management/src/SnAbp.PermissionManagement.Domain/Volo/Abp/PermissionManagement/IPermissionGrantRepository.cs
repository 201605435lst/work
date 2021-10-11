using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.PermissionManagement
{
    public interface IPermissionGrantRepository : IBasicRepository<PermissionGrant, Guid>, IQueryable<PermissionGrant>
    {
        Task<PermissionGrant> FindAsync(
            string name,
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default
        );
        /// <summary>
        /// Easten新增 查询权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="providerName"></param>
        /// <param name="providerGuid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<PermissionGrant> FindAsync(
            string name,
            string providerName,
            Guid providerGuid,
            CancellationToken cancellationToken = default
        );
        Task<List<PermissionGrant>> GetListAsync(
            string providerName,
            string providerKey,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// Easten 新增  根据名称和Guid 获取权限集合
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="providerGuid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<PermissionGrant>> GetListAsync(
            string providerName,
            Guid providerGuid,
            CancellationToken cancellationToken = default
        );

        Task<List<string>> GetGrantNameAsync(
            List<Guid> providerGuids,
            CancellationToken cancellationToken = default
        );

        Task Delete(Expression<Func<PermissionGrant, bool>> condition);
    }
}