using SnAbp.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Entities
{
    /// <summary>
    /// 年月表计划测试项
    /// </summary>
    public class YearMonthPlanTestItem : Entity<Guid>, IRepairTag
    {
        protected YearMonthPlanTestItem() { }
        public YearMonthPlanTestItem(Guid id)
        {
            Id = id;
        }

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

        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid? FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        //排序
        public int? Order { get; set; }

        public Guid? RepairTagId { get; set; }
        public virtual DataDictionary RepairTag { get; set; }

        public bool IsDeleted { get; set; }
    }
}
