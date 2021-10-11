using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 天窗计划明细实体
    /// </summary>
    public class SkylightPlanDailyCompletionDto : IRepairTagDto
    {

        /// <summary>
        /// 天窗Id
        /// </summary>
        public Guid SkylightPlanId { get; set; }
        /// <summary>
        /// 计划数量(日任务)
        /// </summary>
        public decimal PlanCount { get; set; }


        /// <summary>
        /// 完成数量(日任务)
        /// </summary>
        public decimal FinishCount { get; set; }

        /// <summary>
        /// 未完成数量(日任务)
        /// </summary>
        public decimal UnFinishedCount { get; set; }

        /// <summary>
        /// 计划类型（天窗）
        /// </summary>
        public PlanType PlanType { get; set; }
        /// <summary>
        /// 计划状态（天窗）
        /// </summary>
        public PlanState PlanState { get; set; }

        /// <summary>
        /// 计划日期（天窗）
        /// </summary>
        public string PlanTime { get; set; }

        /// <summary>
        /// 车站（区间）名称（天窗）
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 作业时间(派工单)
        /// </summary>
        public string WorkTime { get; set; }

        /// <summary>
        /// 作业组长(派工单)
        /// </summary>
        public string WorkLeader { get; set; }

        /// <summary>
        /// 检修单位(派工单)
        /// </summary>
        public string MaintenanceUnit { get; set; }

        /// <summary>
        /// 验收单位(派工单)
        /// </summary>
        public string CommunicationUnit { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
