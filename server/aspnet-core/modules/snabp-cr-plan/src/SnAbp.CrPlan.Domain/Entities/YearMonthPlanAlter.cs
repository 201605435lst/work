using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 年月表变更记录
    /// </summary>
    public class YearMonthPlanAlter : Entity<Guid>, IRepairTag
    {
        protected YearMonthPlanAlter() { }
        public YearMonthPlanAlter(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 计划主键
        /// </summary>
        public Guid PlanId { get; set; }

        /// <summary>
        /// 开始执行年
        /// </summary>
        public int ExecYear { get; set; }

        /// <summary>
        /// 开始执行月份
        /// </summary>
        public int ExecMonth { get; set; }

        /// <summary>
        /// 是否导入过文件(导入文件标志，防止生成后直接提交)
        /// </summary>
        public bool? IsImport { get; set; }

        /// <summary>
        /// 天窗类型 
        /// </summary>
        [MaxLength(100)]
        public string SkyligetType { get; set; }

        /// <summary>
        /// 设备处所
        /// </summary>
        [MaxLength(500)]
        public string EquipmentLocation { get; set; }

        /// <summary>
        /// 编制执行单位
        /// </summary>
        [MaxLength(100)]
        public string CompiledOrganization { get; set; }

        /// <summary>
        /// 生成类型(年表,月表,年度月表)
        /// </summary>
        public int PlanType { get; set; }

        /// <summary>
        /// 是否已经执行
        /// </summary>
        public int IsExec { get; set; }

        /// <summary>
        /// 状态（未提交、待审核、审核中、审核完成）
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 审批批号
        /// </summary>
        public Guid? AR_Key { get; set; }

        /// <summary>
        /// 制表车间
        /// </summary>
        public Guid WorkShop { get; set; }

        /// <summary>
        /// 附件文件名称
        /// </summary>
        [MaxLength(200)]
        public string FileName { get; set; }

        /// <summary>
        /// 附件文件地址
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// 执表人
        /// </summary>
        [MaxLength(20)]
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
        /// 总数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Total { get; set; }

        /// <summary>
        /// 计划数量
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal PlanCount { get; set; }

        /// <summary>
        /// 列1(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_1 { get; set; }

        /// <summary>
        /// 列2(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_2 { get; set; }

        /// <summary>
        /// 列3(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_3 { get; set; }

        /// <summary>
        /// 列4(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_4 { get; set; }

        /// <summary>
        /// 列5(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_5 { get; set; }

        /// <summary>
        /// 列6(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_6 { get; set; }

        /// <summary>
        /// 列7(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_7 { get; set; }

        /// <summary>
        /// 列8(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_8 { get; set; }

        /// <summary>
        /// 列9(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_9 { get; set; }

        /// <summary>
        /// 列10(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_10 { get; set; }

        /// <summary>
        /// 列11(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_11 { get; set; }

        /// <summary>
        /// 列12(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_12 { get; set; }

        /// <summary>
        /// 列13(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_13 { get; set; }

        /// <summary>
        /// 列14(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_14 { get; set; }

        /// <summary>
        /// 列15(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_15 { get; set; }

        /// <summary>
        /// 列16(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_16 { get; set; }

        /// <summary>
        /// 列17(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_17 { get; set; }

        /// <summary>
        /// 列18(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_18 { get; set; }

        /// <summary>
        /// 列19(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_19 { get; set; }

        /// <summary>
        /// 列20(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_20 { get; set; }

        /// <summary>
        /// 列21(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_21 { get; set; }

        /// <summary>
        /// 列22(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_22 { get; set; }

        /// <summary>
        /// 列23(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_23 { get; set; }

        /// <summary>
        /// 列24(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_24 { get; set; }

        /// <summary>
        /// 列25(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_25 { get; set; }

        /// <summary>
        /// 列26(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_26 { get; set; }

        /// <summary>
        /// 列27(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_27 { get; set; }

        /// <summary>
        /// 列28(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_28 { get; set; }

        /// <summary>
        /// 列29(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_29 { get; set; }

        /// <summary>
        /// 列30(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_30 { get; set; }

        /// <summary>
        /// 列31(月份/日)
        /// </summary>
        [Column(TypeName = "decimal(13, 3)")]
        public decimal Col_31 { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        [MaxLength(500)]
        public string ChangeReason { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public Guid? YearMonthAlterRecordId { get; set; }
    }
}
