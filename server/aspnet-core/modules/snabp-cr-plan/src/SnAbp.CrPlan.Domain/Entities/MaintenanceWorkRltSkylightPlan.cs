using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    public class MaintenanceWorkRltSkylightPlan : Entity<Guid>
    {
        /// <summary>
        /// 维修计划
        /// </summary>
        public Guid MaintenanceWorkId { get; set; }
        public virtual MaintenanceWork MaintenanceWork { get; set; }

        /// <summary>
        /// 计划
        /// </summary>
        public Guid SkylightPlanId { get; set; }
        public virtual SkylightPlan SkylightPlan { get; set; }

        /// <summary>
        /// 作业单位及负责人
        /// </summary>
        public string WorkOrgAndDutyPerson { get; set; }

        /// <summary>
        /// 签收单位
        /// </summary>
        public string SignOrganization { get; set; }

        /// <summary>
        /// 初审部门
        /// </summary>
        public string FirstTrial { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public MaintenanceWorkRltSkylightPlan() {}

        public MaintenanceWorkRltSkylightPlan(Guid id)
        {
            Id = id;
        }
    }
}
