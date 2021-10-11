/**********************************************************************
*******命名空间： SnAbp.Material.Repository
*******类 名 称： MaterialRepository
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/2/3 17:40:47
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Material.Entities;
using SnAbp.Material.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Material.Repository
{
    /// <summary>
    /// $$
    /// </summary>
    public class MaterialRepository : EfCoreRepository<MaterialDbContext, Inventory, Guid>, IMaterialRepository
    {
        public MaterialRepository(IDbContextProvider<MaterialDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task InsertRange<T>(List<T> range) where T:class
        {
            var dbSet = DbContext.Set<T>();
            await dbSet.AddRangeAsync(range);
            await DbContext.SaveChangesAsync();
        }
    }
}
