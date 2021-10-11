using SnAbp.CrPlan.Dto.Worker;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工单实体
    /// 修改派工单使用（无天窗内部数据操作）
    /// </summary>
    public class WorkOrderUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime StartPlanTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime EndPlanTime { get; set; }

        /// <summary>
        /// 作业组长
        /// </summary>
        public Guid WorkLeader { get; set; }

        /// <summary>
        /// 作业成员
        /// </summary>
        public List<Guid> WorkMemberList { get; set; }

        /// <summary>
        /// 现场防护员
        /// </summary>
        public List<Guid> FieldGuardList { get; set; }

        /// <summary>
        /// 驻站联络员
        /// </summary>
        public List<Guid> StationLiaisonOfficerList { get; set; }

        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 通信工具检查情况
        /// </summary>
        public string ToolSituation { get; set; }
    }
}
