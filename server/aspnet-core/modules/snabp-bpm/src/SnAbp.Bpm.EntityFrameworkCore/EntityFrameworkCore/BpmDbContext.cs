using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.Entities;
//using SnAbp.Bpm.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    [ConnectionStringName(BpmDbProperties.ConnectionStringName)]
    public class BpmDbContext : AbpDbContext<BpmDbContext>, IBpmDbContext
    {
        public DbSet<WorkflowTemplate> WorkflowTemplate { get; set; }
        public DbSet<WorkflowTemplateRltMember> WorkflowTemplateMember { get; set; }

        public DbSet<FormTemplate> FormTemplate { get; set; }
        public DbSet<FlowTemplate> FlowTemplate { get; set; }
        public DbSet<FlowTemplateNode> FlowTemplateNode { get; set; }
        public DbSet<FlowTemplateNodeRltMember> FlowTemplateNodeRltMember { get; set; }
        public DbSet<FlowTemplateStep> FlowTemplateStep { get; set; }

        public DbSet<Workflow> Workflow { get; set; }
        public DbSet<WorkflowData> WorkflowData { get; set; }
        public DbSet<WorkflowStateRltMember> WorkflowStateRltMember { get; set; }
        public DbSet<WorkflowTemplateGroup> WorkflowTemplateGroup { get; set; }
        public BpmDbContext(DbContextOptions<BpmDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureBpm();
            
        }
    }
}