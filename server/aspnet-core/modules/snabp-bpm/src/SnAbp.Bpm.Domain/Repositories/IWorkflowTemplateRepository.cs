using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.Bpm.Repositories
{
    public interface IWorkflowTemplateRepository : IRepository<WorkflowTemplate, Guid>
    {
        IQueryable<WorkflowTemplate> GetByKey(string key);
        Task<Guid> GetGroupId();
    }
}
