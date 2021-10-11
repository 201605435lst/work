using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 年月表计划测试项
    /// </summary>
    public class YearMonthPlanTestItemDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 维修项主键
        /// </summary>
        public Guid RepairDetailsID { get; set; }

        /// <summary>
        /// 计划年份
        /// </summary>
        public int PlanYear { get; set; }

        /// <summary>
        /// 测试项名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 测试项类型
        /// </summary>
        public int TestType { get; set; }

        /// <summary>
        /// 测试单位
        /// </summary>
        [MaxLength(50)]
        public string TestUnit { get; set; }

        /// <summary>
        /// 测试项内容
        /// </summary>
        [MaxLength(500)]
        public string TestContent { get; set; }

        /// <summary>
        /// 预设值
        /// </summary>
        [MaxLength(5000)]
        public string PredictedValue { get; set; }

        /// <summary>
        /// 额定值上
        /// </summary>
        public float? MaxRated { get; set; }

        /// <summary>
        /// 额定值下
        /// </summary>
        public float? MinRated { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
