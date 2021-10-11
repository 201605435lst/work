using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 维修项统计Dto（三级：维修项分类---维修项名称---维修项内容）
    /// </summary>
    public class EquipmentStatisticalDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 设备类型名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 已完成数量
        /// </summary>
        public decimal FinishCount { get; set; }

        /// <summary>
        /// 未完成数量
        /// </summary>
        public decimal UnFinishedCount { get; set; }

        /// <summary>
        /// 已变更数量
        /// </summary>
        public decimal ChangeCount { get; set; }

        /// <summary>
        /// 下级
        /// </summary>
        public List<EquipmentStatisticalDto> Children { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
