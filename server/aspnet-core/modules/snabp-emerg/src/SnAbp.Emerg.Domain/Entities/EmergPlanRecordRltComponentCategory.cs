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
    public class EmergPlanRecordRltComponentCategory : Entity<Guid>
    {
        protected EmergPlanRecordRltComponentCategory() { }
        public EmergPlanRecordRltComponentCategory(Guid id) { Id = id; }

        /// <summary>
        /// 预案id
        /// </summary>
        public virtual Guid EmergPlanRecordId { get; set; }
        public virtual EmergPlanRecord EmergPlanRecord { get; set; }

        /// <summary>
        /// 构件id
        /// </summary>
        public virtual Guid ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
