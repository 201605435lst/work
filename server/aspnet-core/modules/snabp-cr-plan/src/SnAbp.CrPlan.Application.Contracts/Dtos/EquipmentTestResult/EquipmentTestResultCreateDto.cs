using SnAbp.Basic.Enums;
using SnAbp.CrPlan.Dtos;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Dto.EquipmentTestResult
{
    /// <summary>
    /// 设备测试项实体，添加使用
    /// </summary>
    public class EquipmentTestResultCreateDto : Entity<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 关联设备表ID
        /// </summary>
        public Guid PlanRelateEquipmentId { get; set; }

        /// <summary>
        /// 测试项ID
        /// </summary>
        public Guid TestId { get; set; }

        /// <summary>
        /// 测试项名称
        /// </summary>
        [MaxLength(100)]
        public string TestName { get; set; }

        /// <summary>
        /// 测试项类型 
        /// </summary>
        public RepairTestType TestType { get; set; }

        // <summary>
        ///预设值 
        /// </summary>
        public string PredictedValue { get; set; }

        /// <summary>
        ///额定值上
        /// </summary>
        public decimal MaxRated { get; set; }

        /// <summary>
        ///额定值下
        /// </summary>
        public decimal MinRated { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}

