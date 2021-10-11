using SnAbp.MultiProject.MultiProject;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Emerg.Entities
{
    /// <summary>
    /// 应急预案-构件分类关联表
    /// </summary>
    public class EmergPlanRltComponentCategory : Entity<Guid>
    {
        protected EmergPlanRltComponentCategory() { }
        public EmergPlanRltComponentCategory(Guid id) { Id = id; }

        /// <summary>
        /// 预案id
        /// </summary>
        public virtual Guid EmergPlanId { get; set; }
        public virtual EmergPlan EmergPlan { get; set; }

        /// <summary>
        /// 构件id
        /// </summary>
        public virtual Guid ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
