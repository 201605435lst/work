using SnAbp.StdBasic.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace SnAbp.StdBasic.Entities
{

    /// <summary>
    /// 测试项表
    /// </summary>
    public class RepairTestItem : Entity<Guid>
    {
        protected RepairTestItem() { }
        public RepairTestItem(Guid id) { Id = id; }

        /// <summary>
        /// 维修项id
        /// </summary>
        public Guid RepairItemId { get; set; }
        public virtual RepairItem RepairItem { get; set; }

        /// <summary>
        /// 测试项名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 测试项类型
        /// </summary>
        public RepairTestType Type { get; set; }

        /// <summary>
        /// 测试单位
        /// </summary>
        [MaxLength(50)]
        public string Unit { get; set; }

        /// <summary>
        /// 预设值
        /// </summary>
        [MaxLength(500)]
        public string DefaultValue { get; set; }

        /// <summary>
        /// 阈值最大值
        /// </summary>
        public float? MaxRated { get; set; }

        /// <summary>
        /// 阈值最小值
        /// </summary>
        public float? MinRated { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public int? Order { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid? FileId { get; set; }
        public virtual SnAbp.File.Entities.File File { get; set; }
    }
}
