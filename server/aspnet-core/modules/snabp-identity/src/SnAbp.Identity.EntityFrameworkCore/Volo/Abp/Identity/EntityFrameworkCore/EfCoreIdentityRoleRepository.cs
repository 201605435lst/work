using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IIdentityDbContext, IdentityRole, Guid>, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<IdentityRole> FindByNormalizedNameAsync(
            string normalizedRoleName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var data = DbSet
                .IncludeDetails(includeDetails);
            if (data.Any()) return data.FirstOrDefault(r => r.NormalizedName == normalizedRoleName);
            else return null;
            //return await
            //    .FirstOrDefaultAsync(, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(
            List<Guid> limitGuids = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var query = DbContext.Set<OrganizationRltRole>();
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                        x => x.Name.Contains(filter) ||
                        x.NormalizedName.Contains(filter))
                .WhereIf(limitGuids!=null,a=>limitGuids.Contains(a.Id)||a.IsPublic)
                .WhereIf(limitGuids == null, a => !query.Select(b => b.RoleId).Contains(a.Id)) //��ȡ���������κ���֯�����Ľ�ɫ
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetListAsync(
            IEnumerable<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(t => ids.Contains(t.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<IdentityRole>> GetDefaultOnesAsync(
            bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet.IncludeDetails(includeDetails).Where(r => r.IsDefault).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
            List<Guid>limitGuids=null,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query=DbContext.Set<OrganizationRltRole>();
                
            return await DbSet
                .WhereIf(!filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(filter) ||
                         x.NormalizedName.Contains(filter))
                .WhereIf(limitGuids!=null,a=>limitGuids.Contains(a.Id)||a.IsPublic)
                .WhereIf(limitGuids==null,a=> !query.Select(b => b.RoleId).Contains(a.Id)) //��ȡ���������κ���֯�����Ľ�ɫ
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }

        public override IQueryable<IdentityRole> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public async Task<List<IdentityRole>> GetListByOrganizationIdsAsync(List<Guid> organizationIds = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, string filter = null, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var roleIds = await DbContext.Set<OrganizationRltRole>()
                .Where(a => organizationIds.Contains(a.OrganizationId))
                .Select(a => a.RoleId)
                .ToListAsync(cancellationToken);
            return await DbSet.Where(a => a.IsPublic&&roleIds.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// ��ȡ��֯�ṹ�����Ľ�ɫ���������еĽ�ɫ
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
       
        public async Task<List<IdentityRole>> GetListAsync(Guid? organizationId, CancellationToken cancellationToken = default)
        {
            if (organizationId == null) return null;
            var roleIds = await DbContext.Set<OrganizationRltRole>()
                .Where(a => a.OrganizationId == organizationId)
                .Select(a => a.RoleId).ToListAsync(cancellationToken: cancellationToken);
            return await DbSet.Where(a => roleIds.Contains(a.Id))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// ��ȡϵͳ��ɫ
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<IdentityRole>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
        {
            var roleIds = DbContext.Set<OrganizationRltRole>()
                .Select(a => a.RoleId);
            return await DbSet.Where(a => !roleIds.Contains(a.Id)&&a.IsPublic)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> CheckSameRoleName(Guid organizationId,string roleName)
        {
            var roleIds = DbContext.Set<OrganizationRltRole>()
                .Where(a => a.OrganizationId == organizationId)
                .Select(a => a.RoleId);
            if (!await roleIds.AnyAsync()) return false;
            var role =await DbSet.Where(a => roleIds.Contains(a.Id))
                .AnyAsync(a => a.NormalizedName == roleName);
            return role;
        }

        public async Task<bool> CheckSameRoleName(string roleName)
        {
            //先查出同名的角色
            var roles = DbSet.Where(a => a.NormalizedName == roleName);
            if (roles.Any())
            {
                var roleIds = roles.Select(a => a.Id).ToList();
                // 判断组织机构关联表中是否有数据
                var rlts =await DbContext.Set<OrganizationRltRole>()
                    .CountAsync(a => roleIds.Any(b => b == a.RoleId));
                return roleIds.Count == rlts;
            }
            else
            {
                // 不存在
                return true;
            }
        }
    }
}