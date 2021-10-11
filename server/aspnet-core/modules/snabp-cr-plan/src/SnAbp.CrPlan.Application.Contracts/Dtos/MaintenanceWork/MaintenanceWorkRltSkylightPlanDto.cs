using SnAbp.CrPlan.Dto.SkylightPlan;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class MaintenanceWorkRltSkylightPlanDto : EntityDto<Guid>
    {
        /// <summary>
        /// 维修计划
        /// </summary>
        public Guid MaintenanceWorkId { get; set; }

        /// <summary>
        /// 计划
        /// </summary>
        public Guid SkylightPlanId { get; set; }
        public virtual SkylightPlanDto SkylightPlan { get; set; }

        /// <summary>
        /// 维修项目
        /// </summary>
        public string MaintenanceProject { get; set; }

        /// <summary>
        /// 维修类型
        /// </summary>
        public PlanType MaintenanceType { get; set; }

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
    }
}
