using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 月表完成率实体
    /// </summary>
    public class MonthCompletionRatesDto : IRepairTagDto
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 设备处所
        /// </summary>
        public string EquipmentLocation { get; set; }

        /// <summary>
        /// 设备名称(维修项设备)
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string RepairContent { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public decimal FinishCount { get; set; }

        /// <summary>
        /// 完成百分比
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// 差距
        /// </summary>
        public decimal Gap { get; set; }

        /// <summary>
        /// 变更数量
        /// </summary>
        public decimal ChangeCount { get; set; }

        /// <summary>
        /// 未完成数量
        /// </summary>
        public decimal UnFinishedCount { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public List<MonthStatisticalDto> DetailList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
