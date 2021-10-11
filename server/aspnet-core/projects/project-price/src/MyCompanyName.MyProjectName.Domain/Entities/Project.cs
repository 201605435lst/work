using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyCompanyName.MyProjectName.Entities
{
    /// <summary>
    /// 项目
    /// </summary>
    public class Project : AuditedEntity<Guid>, IGuidKeyTree<Project>
    {
        protected Project() { }
        public Project(Guid id) { Id = id; }

        public Guid? ParentId { get; set; }
        public Project Parent { get; set; }
        public List<Project> Children { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        public float SumPrice { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        public string Description { get; set; }


        [InverseProperty("Project")]
        public List<ProjectRltModule> ProjectRltModules { get; set; }
    }
}
