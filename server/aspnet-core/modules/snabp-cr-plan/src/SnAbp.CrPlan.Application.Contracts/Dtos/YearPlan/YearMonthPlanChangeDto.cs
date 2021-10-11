using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 年月表计划
    /// </summary>
    public class YearMonthPlanChangeDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 维修项主键
        /// </summary>
        public Guid RepairDetailsId { get; set; }

        /// <summary>
        /// 维修项序号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 执行单位
        /// </summary>
        public string ExecutableUnitStr { get; set; }

        /// <summary>
        /// 编制执行单位
        /// </summary>
        [MaxLength(100)]
        public string CompiledOrganization { get; set; }

        /// <summary>
        /// 设备类型(维修项)
        /// </summary>
        [MaxLength(50)]
        public string RepairGroup { get; set; }

        /// <summary>
        /// 类别（集中检修/日常检修）
        /// </summary>
        public int RepairType { get; set; }

        /// <summary>
        /// 设备名称(维修项设备)
        /// </summary>
        [MaxLength(500)]
        public string DeviceName { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        [MaxLength(1000)]
        public string RepairContent { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        [MaxLength(50)]
        public string Unit { get; set; }

        /// <summary>
        /// 设备数量
        /// </summary>
        public decimal DeviceCount { get; set; }

        /// <summary>
        /// 计划类型(年计划、月计划、年表月计划)
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 次数
        /// </summary>
        [MaxLength(50)]
        public string Times { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 负责单位
        /// </summary>
        public Guid ResponsibleUnit { get; set; }

        /// <summary>
        /// 设备处所
        /// </summary>
        public string EquipmentLocation { get; set; }

        /// <summary>
        /// 维修月份
        /// </summary>
        [MaxLength(50)]
        public string RepairMonth { get; set; }

        /// <summary>
        /// 执表人
        /// </summary>
        [MaxLength(50)]
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }

        /// <summary>
        /// 单位工时
        /// </summary>
        public decimal UnitTime { get; set; }

        /// <summary>
        /// 状态（未提交、待审核、审核中、审核完成）
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 状态（未提交、待审核、审核中、审核完成）
        /// </summary>
        public string StateStr { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// 审批批号
        /// </summary>
        public Guid? AR_Key { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 父级类型
        /// </summary>
        public int? ParentType { get; set; }

        /// <summary>
        /// 天窗类型(多个用逗号分隔)
        /// </summary>
        [MaxLength(100)]
        public string SkyligetType { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
