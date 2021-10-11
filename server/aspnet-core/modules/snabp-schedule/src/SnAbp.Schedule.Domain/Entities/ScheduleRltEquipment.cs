using SnAbp.Resource.Entities;
using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Schedule.Entities
{
    public class ScheduleRltEquipment : Entity<Guid>
    {
        public ScheduleRltEquipment(Guid id) => Id = id;
        /// <summary>
        /// 计划Id
        /// </summary>
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State State { get; set; }
    }
}
