using SnAbp.Alarm.Enums;
using SnAbp.Identity;
using SnAbp.Resource.Dtos;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Alarm.Dtos
{
    public class AlarmSimple : EntityDto<string>
    {
        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }
        public EquipmentDto Equipment { get; set; }


        /// <summary>
        /// 告警级别
        /// </summary>
        public int Level { get; set; }


        /// <summary>
        /// 告警编码
        /// </summary>
        public string Code { get; set; }


        ///// <summary>
        ///// 告警名称（告警编码名称）
        ///// </summary>
        //public string Name { get; set; }


        /// <summary>
        /// 告警内容（原因）
        /// </summary>
        public string Content { get; set; }


        /// <summary>
        /// 告警时间
        /// </summary>
        public DateTime ActivedTime { get; set; }


        /// <summary>
        /// 是否已确认，0：未确认，1：已确认
        /// </summary>
        public int IsConfirm { get; set; }
    }
}
