/**********************************************************************
*******命名空间： SnAbp.Message.Bpm.Services
*******类 名 称： BpmMessageRepository
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/11 8:46:15
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SnAbp.Bpm.Entities;
using SnAbp.Bpm.Repositories;
using SnAbp.Message.Bpm.Entities;
using SnAbp.Message.Bpm.EntityFrameworkCore;
using SnAbp.Message.Bpm.Services;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SnAbp.Message.Bpm.Repositorys
{
    public class BpmMessageRepository : EfCoreRepository<BpmMessageDbContext, BpmRltMessage, Guid>, IBpmMessageRepository
    {
        private readonly IDbContextProvider<BpmMessageDbContext> _dbContextProvider;
        public BpmMessageRepository(IDbContextProvider<BpmMessageDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        

        public async Task<List<BpmRltMessage>> GetList(string keyword,bool? isProcess)
        {
            return await
                DbContext.BpmRltMessage
                    .Include(a => a.Workflow.FlowTemplate.FormTemplate.WorkflowTemplate)
                    .Include(a => a.User)
                    .Include(a => a.Processor)
                    .Include(a => a.Sponsor)
                    .WhereIf(!keyword.IsNullOrEmpty(), a => a.Sponsor.Name.Contains(keyword) || a.Processor.Name.Contains(keyword))
                    .WhereIf(isProcess != null, a => a.Process == isProcess)
                    .ToListAsync();
        }

        public async Task<List<BpmRltMessage>> GetNoProcessMessage(string userId)
        {
            return await
                DbContext.BpmRltMessage
                    .Include(a => a.Workflow.FlowTemplate.FormTemplate.WorkflowTemplate)
                    .Include(a => a.User)
                    .Include(a => a.Processor)
                    .Include(a=>a.Sponsor)
                    .WhereIf(!userId.IsNullOrEmpty(), a => a.UserId == Guid.Parse(userId) && !a.Process)
                    .ToListAsync();
        }

        public async Task<BpmRltMessage> Insert(BpmRltMessage model)
        {
            await DbContext.BpmRltMessage.AddAsync(model);
            await DbContext.SaveChangesAsync();
            return await DbContext.BpmRltMessage
                .Include(a => a.Workflow.FlowTemplate.FormTemplate.WorkflowTemplate)
                .Include(a => a.User)
                .Include(a => a.Processor)
                .Include(a => a.Sponsor)
                .FirstOrDefaultAsync(b => b.Id == model.Id);
        }

        public async Task Update(string messageId)
        {
            var entity =await DbContext.BpmRltMessage.FirstOrDefaultAsync(a => a.Id == Guid.Parse(messageId));
            entity.Process = true;
            DbContext.BpmRltMessage.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<BpmRltMessage> UpdateRange(List<Guid> messageIds)
        {
            BpmRltMessage entity = null;
            foreach (var messageId in messageIds)
            {
                entity = await DbContext.BpmRltMessage.FirstOrDefaultAsync(a => a.Id == messageId);
                entity.Process = true;
                DbContext.BpmRltMessage.Update(entity);
                await DbContext.SaveChangesAsync();
            }
            return entity;
        }
        public async Task<bool> Delete(string messageId)
        {
            var entity = await DbContext.BpmRltMessage.FirstOrDefaultAsync(a => a.Id == Guid.Parse(messageId));
            DbContext.BpmRltMessage.Remove(entity);
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}
