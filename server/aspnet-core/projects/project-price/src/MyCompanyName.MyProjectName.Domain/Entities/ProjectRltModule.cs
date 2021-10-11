using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace MyCompanyName.MyProjectName.Entities
{
    /// <summary>
    /// 实际项目模块
    /// </summary>
    public class ProjectRltModule : Entity<Guid>, IGuidKeyTree<ProjectRltModule>
    {
        protected ProjectRltModule() { }
        public ProjectRltModule(Guid id) { Id = id; }

        public Project Project { get; set; }
        public Guid ProjectId { get; set; }

        public Module? Module { get; set; }
        public Guid? ModuleId { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public float Price { get; set; }

        public string Remark { get; set; }
        public Guid? ParentId { get; set; }
        public ProjectRltModule Parent { get; set; }
        public List<ProjectRltModule> Children { get; set; }
    }
}
