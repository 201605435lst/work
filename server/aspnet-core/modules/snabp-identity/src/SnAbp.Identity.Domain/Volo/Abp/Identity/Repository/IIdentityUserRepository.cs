using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace SnAbp.Identity
{
    public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>, IQueryable<IdentityUser>
    {
        Task<IdentityUser> FindByNormalizedUserNameAsync(
            [NotNull] string normalizedUserName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<List<string>> GetRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        Task<List<string>> GetRoleNamesInOrganizationAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<IdentityUser> FindByLoginAsync(
            [NotNull] string loginProvider,
            [NotNull] string providerKey,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        Task<IdentityUser> FindByNormalizedEmailAsync(
            [NotNull] string normalizedEmail,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );
        Task<IdentityUser> FindByUserNameAsync(
            [NotNull] string userName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );
        Task<List<IdentityUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
            string normalizedRoleName,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetListAsync(
            Guid? organizationId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );
        Task<List<IdentityUser>> GetUserListAsync(
           string sorting = null,
           int maxResultCount = int.MaxValue,
           int skipCount = 0,
           Guid? organizationId = null,
           string filter = null,
           bool includeDetails = false,
           CancellationToken cancellationToken = default
       );

        Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        //新增角色查询
        Task<List<IdentityRole>> GetRolesAsync(
       Guid id,
       bool includeDetails = false
   );

        Task<List<Organization>> GetOrganizationsAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        Task<List<IdentityUser>> GetUsersInOrganizationAsync(
            Guid organizationId,
            CancellationToken cancellationToken = default
            );
        Task<List<IdentityUser>> GetUserListAsync(
          Expression<Func<IdentityUser, bool>> func,
          CancellationToken cancellationToken = default
          );

        Task DeleteAsync(
         Expression<Func<IdentityUser, bool>> func,
         CancellationToken cancellationToken = default
         );

        Task DeleteAsync(
          Expression<Func<IdentityUserRltOrganization, bool>> func,
          CancellationToken cancellationToken = default
        );
        Task CreateUserOrganization(
          IdentityUserRltOrganization model,
          CancellationToken cancellationToken = default
        );
        Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationIds,
            CancellationToken cancellationToken = default
            );

        Task<List<IdentityUser>> GetUsersInOrganizationWithChildrenAsync(
            string code,
            CancellationToken cancellationToken = default
            );

        Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default
        );

        Task<List<IdentityUser>> GetUserList(Expression<Func<IdentityUser, bool>> func, bool includeDetail = false);

        Task<bool> CreateUserAndProject(IdentityUserRltProject userRltProject);
    }
}