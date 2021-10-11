using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Entities;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Alarm.Entities
{
    /// <summary>
    /// 告警配置
    /// </summary>
    public class AlarmConfig : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        public AlarmConfig() { }
        public AlarmConfig(Guid id) { Id = id; }


        /// <summary>
        /// 构件分类
        /// </summary>
        public Guid ComponentCategoryId { get; set; }
        public ComponentCategory ComponentCategory { get; set; }


        /// <summary>
        /// 最大值
        /// </summary>
        public decimal MaxValue { get; set; }


        /// <summary>
        /// 最小值
        /// </summary>
        public decimal MinValue { get; set; }
    }
}
