using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Bpm.Repositories
{
    public class WorkflowTemplateRepository : EfCoreRepository<BpmDbContext, WorkflowTemplate, Guid>, IWorkflowTemplateRepository
    {
        private readonly IDbContextProvider<BpmDbContext> _dbContextProvider;

        public WorkflowTemplateRepository(IDbContextProvider<BpmDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;

        }

        public IQueryable<WorkflowTemplate> GetByKey(string key)
        {
            var ctx = _dbContextProvider.GetDbContext();
            IQueryable<WorkflowTemplate> query = null;

            query = from wt in ctx.WorkflowTemplate
                    where wt.Key == key
                    select wt;

            return query;
        }

        public async Task<Guid> GetGroupId()
        {
            var group =await DbContext.WorkflowTemplateGroup.FirstOrDefaultAsync(a => a.Name == "系统流程");
            if (@group != null)
            {
                return @group.Id;
            }
            @group = new WorkflowTemplateGroup {Name = "系统流程", Order = 0};
            @group.SetId(Guid.NewGuid());
            await DbContext.WorkflowTemplateGroup.AddAsync(@group);
            await DbContext.SaveChangesAsync(default);
            return group.Id;
        }
    }
}
