using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Utils.TreeHelper;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Problem.Entities
{
    public class ProblemCategory : FullAuditedEntity<Guid>, IGuidKeyTree<ProblemCategory> //FullAudited
    {
        /// <summary>
        /// 全称 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序 
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        public ProblemCategory Parent { get; set; }
        public List<ProblemCategory> Children { get; set; }

        protected ProblemCategory() { }
        public ProblemCategory(Guid id) { Id = id; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
