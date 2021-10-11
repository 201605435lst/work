using SnAbp.MultiProject.MultiProject;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Emerg.Entities
{
    public class FaultRltComponentCategory : Entity<Guid>
    {
        protected FaultRltComponentCategory() { }
        public FaultRltComponentCategory(Guid id) { Id = id; }

        /// <summary>
        /// 故障
        /// </summary>
        public Guid FaultId { get; set; }
        public virtual Fault Fault { get; set; }

        /// <summary>
        /// 构件
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public virtual ComponentCategory ComponentCategory { get; set; }
        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
