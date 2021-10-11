using SnAbp.CrPlan.Dtos;
using SnAbp.File.Dtos;
using SnAbp.Identity;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Dto.EquipmentTestResult
{
    /// <summary>
    /// 设备的测试项实体，获取数据使用
    /// 本层为WorkOrderDetailedDto数据第六层
    /// </summary>
    public class EquipmentTestResultDto : Entity<Guid>, IRepairTagDto
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
        /// 测试结果
        /// </summary>
        [MaxLength(100)]
        public string TestResult { get; set; }

        /// <summary>
        /// 验收结果
        /// </summary>
        [MaxLength(100)]
        public string CheckResult { get; set; }

        /// <summary>
        /// 测试项类型 
        /// </summary>
        public RepairTestType TestType { get; set; }

        /// <summary>
        ///预设值 
        /// </summary>
        public List<string> PredictedValue { get; set; }

        /// <summary>
        ///额定值上
        /// </summary>
        public decimal MaxRated { get; set; }

        /// <summary>
        ///额定值下
        /// </summary>
        public decimal MinRated { get; set; }

        /// <summary>
        /// 上传文件Id
        /// </summary>
        public virtual Guid? FileId { get; set; }

        /// <summary>
        /// 上传文件
        /// </summary>
        public virtual FileSimpleDto File { get; set; }

        public Guid? RepairTagId { get; set; }

        public DataDictionaryDto RepairTag { get; set; }

        public string Unit { get; set; }

        public int? Order { get; set; }
        public EquipmentTestResultDto() { }
        public EquipmentTestResultDto(Guid id) { Id = id; }
    }
}
