using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.Statistical
{
    public class PlanStateTrackingDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 设备名称(维修项设备)
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string RepairContent { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 天窗类型(多个用逗号分隔)
        /// </summary>
        public string SkyligetType { get; set; }

        /// <summary>
        /// 计划完成情况列表
        /// </summary>
        public List<PlanCompletionDto> PlanCompletionList { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
