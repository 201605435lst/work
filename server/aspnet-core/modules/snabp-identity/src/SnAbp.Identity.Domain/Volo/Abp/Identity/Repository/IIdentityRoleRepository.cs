using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Identity
{
    public interface IIdentityRoleRepository : IBasicRepository<IdentityRole, Guid>, IQueryable<IdentityRole>
    {
        Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetListAsync(
            List<Guid> limitGuids = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
        Task<List<IdentityRole>> GetListByOrganizationIdsAsync(
            List<Guid> organizationIds = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );


        Task<List<IdentityRole>> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default
        );
        Task<List<IdentityRole>> GetListAsync(
            Guid? organizationId,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetSystemRolesAsync(
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityRole>> GetDefaultOnesAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            List<Guid> limitGuids = null,
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<bool> CheckSameRoleName(Guid organizationId, string roleName);
        Task<bool> CheckSameRoleName(string roleName);
    }
}