using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enumer;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.AlterRecord
{
    public class DailyPlanAlterDetailDto : EntityDto<Guid>, IRepairTagDto
    {
        public Guid PlanId { get; set; }

        /// <summary>
        /// 变更类型
        /// </summary>
        public SelectablePlanType PlanType { get; set; }
        public string PlanTypeStr { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipName { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 变更数量
        /// </summary>
        public decimal AlterCount { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        public DateTime AlterDateTime { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
