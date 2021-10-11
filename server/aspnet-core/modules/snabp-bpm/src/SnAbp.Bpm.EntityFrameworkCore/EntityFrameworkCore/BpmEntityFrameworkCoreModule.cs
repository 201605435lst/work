using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SnAbp.Bpm.Entities;
//using SnAbp.Bpm.Entities;
//using SnAbp.Bpm.Repositories;
using SnAbp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    [DependsOn(
        typeof(BpmDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BpmEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BpmDbContext>(options =>
            {
                options.AddDefaultRepositories<IBpmDbContext>(true);

                options.Entity<WorkflowTemplate>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.FormTemplates).ThenInclude(y => y.FlowTemplates).ThenInclude(z => z.Workflows)
                        .Include(x => x.FormTemplates).ThenInclude(y => y.FlowTemplates).ThenInclude(z => z.Nodes).ThenInclude(m => m.Members)
                        .Include(x => x.FormTemplates).ThenInclude(y => y.FlowTemplates).ThenInclude(z => z.Steps)
                        .Include(x => x.Members)
                    );

                options.Entity<Workflow>(x => x.DefaultWithDetailsFunc = q => q
                        .Include(x => x.FlowTemplate.FormTemplate.WorkflowTemplate)
                        .Include(x => x.FlowTemplate.Steps)
                        .Include(x => x.FlowTemplate.Nodes).ThenInclude(y => y.Members)
                        .Include(x => x.WorkflowDatas)
                    );
                options.Entity<WorkflowStateRltMember>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Workflow).ThenInclude(y => y.FlowTemplate.FormTemplate.WorkflowTemplate)
                    .Include(x => x.Workflow).ThenInclude(y => y.FlowTemplate.Steps)
                    .Include(x => x.Workflow).ThenInclude(y => y.FlowTemplate.Nodes).ThenInclude(y => y.Members)
                    .Include(x => x.Workflow).ThenInclude(y => y.WorkflowDatas)
                );
                options.Entity<SingleFlowEntity>(x => x.DefaultWithDetailsFunc = q => q
                    .Include(x => x.Workflow).ThenInclude(y => y.FlowTemplate)
                );
            });
        }
    }
}