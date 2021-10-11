using SnAbp.Identity;
using SnAbp.StdBasic.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 设备测试项结果实体
    /// </summary>
    public class EquipmentTestResult : Entity<Guid>, IRepairTag
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
        [StringLength(100)]
        public string TestName { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        [StringLength(100)]
        public string TestResult { get; set; }

        /// <summary>
        /// 验收结果
        /// </summary>
        [StringLength(100)]
        public string CheckResult { get; set; }

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

        public virtual Guid? FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }


        /// <summary>
        ///额定值下
        /// </summary>
        public decimal MinRated { get; set; }
        public EquipmentTestResult() { }
        public EquipmentTestResult(Guid id) { Id = id; }

    }
}
