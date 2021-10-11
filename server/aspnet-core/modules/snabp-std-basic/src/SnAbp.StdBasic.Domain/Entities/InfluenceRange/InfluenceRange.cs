using SnAbp.Identity;
using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.StdBasic.Entities
{
    /// <summary>
    /// 影响范围实体
    /// </summary>
    public class InfluenceRange : Entity<Guid>
    {
        /// <summary>
        /// 维修级别
        /// </summary>
        public SkylightPlanRepairLevel RepairLevel { get; set; }

        /// <summary>
        /// 影响范围内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public Guid? TagId { get; set; }
        public virtual DataDictionary Tag { get; set; }

        public InfluenceRange(Guid id)
        {
            Id = id;
        }
    }
}
