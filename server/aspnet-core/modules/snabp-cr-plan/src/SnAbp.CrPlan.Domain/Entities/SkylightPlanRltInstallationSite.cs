using SnAbp.Basic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    public class SkylightPlanRltInstallationSite : Entity<Guid>
    {
        /// <summary>
        /// 天窗计划
        /// </summary>
        public Guid SkylightPlanId { get; set; }
        public virtual SkylightPlan SkylightPlan { get; set; }

        /// <summary>
        /// 天窗计划
        /// </summary>
        public Guid InstallationSiteId { get; set; }
        public virtual InstallationSite InstallationSite { get; set; }

        public SkylightPlanRltInstallationSite() {}

        public SkylightPlanRltInstallationSite(Guid id)
        {
            Id = id;
        }
    }
}
