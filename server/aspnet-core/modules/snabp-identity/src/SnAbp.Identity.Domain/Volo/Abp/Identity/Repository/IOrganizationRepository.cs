using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Identity
{
    public interface IOrganizationRepository : IBasicRepository<Organization, Guid>, IQueryable<Organization>, IReadOnlyRepository<Organization>
    {
        Task<List<Organization>> GetChildrenAsync(
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<Organization>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<Organization> GetAsync(
            string displayName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<Organization> GetAsync(
            Expression<Func<Organization, bool>> expression,
            CancellationToken cancellationToken = default
        );

        Task<List<Organization>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<Organization>> GetListAsync(
            IEnumerable<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetRolesAsync(
            Organization organization,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<int> GetRolesCountAsync(
            Organization organization,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetMembersAsync(
            Organization organization,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<int> GetMembersCountAsync(
            Organization organization,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task RemoveAllRolesAsync(
            Organization organization,
            CancellationToken cancellationToken = default
        );

        Task RemoveAllMembersAsync(
            Organization organization,
            CancellationToken cancellationToken = default
        );

        Task<IQueryable<Organization>> Where(Expression<Func<Organization, bool>> func);
        Task UpdateRanges(IEnumerable<Organization> organizations, CancellationToken cancellationToken = default);
        Task InsertRanges(IEnumerable<Organization> organizations, CancellationToken cancellationToken = default);



    }
}