/**********************************************************************
*******命名空间： Volo.Abp.Identity.EntityFrameworkCore
*******类 名 称： IdentityUserRoleRepository
*******类 说 明： 新增数据库仓库，原生方法不支持用户和角色的关联表维护，
 *                将仓储放在存储层的原因是领域层无法进行依赖注入，只能通过构造函数的形式进行赋值
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/4 15:51:43
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SnAbp.Identity;
using SnAbp.Identity.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Identity.EntityFrameworkCore
{
    public class IdentityUserRoleRepository : EfCoreRepository<IIdentityDbContext,IdentityUserRltRole>,IIdentityUserRoleRepository
    {
        public IdentityUserRoleRepository(IDbContextProvider<IIdentityDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
        public override async Task<List<IdentityUserRltRole>> GetListAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbSet.ToListAsync(cancellationToken: cancellationToken);
        }

        public override async Task<long> GetCountAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await DbSet.CountAsync(cancellationToken);
        }

        public override async Task<IdentityUserRltRole> InsertAsync(IdentityUserRltRole entity, bool autoSave = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            await DbSet.AddAsync(entity, cancellationToken);
            if (autoSave)
            {
                await DbContext.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }

        public override Task<IdentityUserRltRole> UpdateAsync(IdentityUserRltRole entity, bool autoSave = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(IdentityUserRltRole entity, bool autoSave = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Guid>> GetRoleIdsByUser(Guid userIdGuid)
        {
            return DbContext.Set<IdentityUserRltRole>()
                .Where(a => a.UserId == userIdGuid)
                .Select(b => b.RoleId);
        }

        public async Task<IdentityResult> InsertAsync(IdentityUserRltRole userRoleModel, CancellationToken token)
        {
            try
            {
                await DbSet.AddAsync(userRoleModel, token);
                await DbContext.SaveChangesAsync(token);
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                return IdentityResult.Failed();
            }
          
        }

        public async Task DeleteAsync(Expression<Func<IdentityUserRltRole, bool>> func, CancellationToken token)
        {
            var list = DbSet.Where(func).ToDynamicList<IdentityUserRltRole>();

            if (list != null && list.Any())
            {
                DbSet.RemoveRange(list);
            }

            await DbContext.SaveChangesAsync(token);
        }

       
    }
}
