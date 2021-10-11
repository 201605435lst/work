/**********************************************************************
*******命名空间： SnAbp.Bpm.Repositories
*******类 名 称： SingleFlowProcessRepository
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/1/15 15:37:35
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Bpm.Repositories
{
    /// <summary>
    /// $$
    /// </summary>
    public class SingleFlowProcessRepository: EfCoreRepository<BpmDbContext, Workflow, Guid>, ISingleFlowProcessRepository
    {
        public SingleFlowProcessRepository(IDbContextProvider<BpmDbContext> dbContextProvider) : base(dbContextProvider)
        {
          
        }

        public async Task<List<T1>> GetList<T1>(Expression<Func<T1, bool>> func) where  T1:class
        {
            var en = DbContext.Set<T1>();
            return await en.Where(func).ToListAsync<T1>();
        }

        public async Task Insert<T>(T t) where T : class
        {
            DbContext.Set<T>().Add(t);
            await DbContext.SaveChangesAsync();
        }
    }
}
