using SnAbp.Alarm.Enums;
using SnAbp.Identity;
using SnAbp.Resource.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Alarm.Dtos
{
    public class AlarmDto : EntityDto<Guid>
    {
        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public EquipmentDto Equipment { get; set; }


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
