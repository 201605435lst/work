using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Problem.Entities
{
    public class Problem : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 详情
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// 问题分类关联表
        /// </summary>
        [InverseProperty("Problem")]
        public virtual List<ProblemRltProblemCategory> ProblemRltProblemCategories { get; set; }

        protected Problem() { }
        public Problem(Guid id) { Id = id; }
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
