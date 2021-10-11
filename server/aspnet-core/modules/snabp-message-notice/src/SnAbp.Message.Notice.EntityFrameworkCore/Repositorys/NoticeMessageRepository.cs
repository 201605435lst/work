/**********************************************************************
*******命名空间： SnAbp.Message.Notice.Repositorys
*******类 名 称： NoticeMessageRepository
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/25 10:23:20
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnAbp.Message.Notice.EntityFrameworkCore;
using SnAbp.Message.Notice.IRepositorys;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Message.Notice.Repositorys
{
    public class NoticeMessageRepository : EfCoreRepository<NoticeDbContext, Entities.Notice, Guid>,
        INoticeMessageRepository
    {
        readonly IDbContextProvider<NoticeDbContext> _dbContextProvider;

        public NoticeMessageRepository(IDbContextProvider<NoticeDbContext> dbContextProvider) :
            base(dbContextProvider) => _dbContextProvider = dbContextProvider;

        public Task<List<Entities.Notice>> GetNoProcessMessage(string userId) =>

            DbSet.Where(a => a.UserId == Guid.Parse(userId)).ToListAsync();

        public async Task<Entities.Notice> InsertAsync(Entities.Notice data)
        {
            await DbContext.Notice.AddAsync(data);
            await DbContext.SaveChangesAsync();
            return await DbContext.Notice.FirstOrDefaultAsync(a => a.Id == data.Id);
        }
    }
}