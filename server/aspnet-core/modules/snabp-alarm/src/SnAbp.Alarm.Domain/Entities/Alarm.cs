using SnAbp.Alarm.Enums;
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using SnAbp.Resource.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Alarm.Entities
{
    /// <summary>
    /// 告警记录
    /// </summary>
    public class Alarm : AuditedEntity<Guid>
    {
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
        public Alarm() { }
        public Alarm(Guid id) { Id = id; }


        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }


        /// <summary>
        /// 告警级别
        /// </summary>
        public AlarmLevel Level { get; set; }


        /// <summary>
        /// 告警编码
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 告警名称（告警编码名称）
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 告警内容（原因）
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 告警时间
        /// </summary>
        public DateTime ActivedTime { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime ConfirmedTime { get; set; }


        /// <summary>
        /// 消除时间
        /// </summary>
        public DateTime ClearedTime { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        public AlarmState State { get; set; }
    }
}
