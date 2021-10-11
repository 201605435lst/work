using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dtos
{
    public class YearMonthPlanYearUpdateDto : EntityDto<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 天窗类型(多个用逗号分隔)
        /// </summary>
        [MaxLength(100)]
        public string SkyligetType { get; set; }

        public string RepairTagKey { get; set; }
        /// <summary>
        /// 设备处所
        /// </summary>
        public string EquipmentLocation { get; set; }

        /// <summary>
        /// 编制执行单位
        /// </summary>
        [MaxLength(100)]
        public string CompiledOrganization { get; set; }

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

    }
}
