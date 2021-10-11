using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.Entities;
//using SnAbp.Bpm.Entities;
using Volo.Abp.Data;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Bpm.EntityFrameworkCore
{
    [ConnectionStringName(BpmDbProperties.ConnectionStringName)]
    public interface IBpmDbContext : IEfCoreDbContext
    {
        DbSet<WorkflowTemplate> WorkflowTemplate { get; set; }
        DbSet<WorkflowTemplateRltMember> WorkflowTemplateMember { get; set; }

        DbSet<FormTemplate> FormTemplate { get; set; }
        DbSet<FlowTemplate> FlowTemplate { get; set; }
        DbSet<FlowTemplateNode> FlowTemplateNode { get; set; }
        DbSet<FlowTemplateNodeRltMember> FlowTemplateNodeRltMember { get; set; }
        DbSet<FlowTemplateStep> FlowTemplateStep { get; set; }

        DbSet<Workflow> Workflow { get; set; }
        DbSet<WorkflowData> WorkflowData { get; set; }
        DbSet<WorkflowStateRltMember> WorkflowStateRltMember { get; set; }
        DbSet<WorkflowTemplateGroup> WorkflowTemplateGroup { get; set; }
    }
}