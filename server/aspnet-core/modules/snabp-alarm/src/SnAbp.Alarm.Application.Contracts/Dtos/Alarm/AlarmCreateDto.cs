using SnAbp.Alarm.Enums;
using SnAbp.Identity;
using SnAbp.Resource.Dtos;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Alarm.Dtos
{
    public class AlarmCreateDto
    {
        [Required]
        /// <summary>
        /// 设备
        /// </summary>
        public Guid EquipmentId { get; set; }


        [Required]
        /// <summary>
        /// 告警级别
        /// </summary>
        public AlarmLevel Level { get; set; }


        [Required]
        /// <summary>
        /// 告警编码
        /// </summary>
        public string Code { get; set; }


        [Required]
        /// <summary>
        /// 告警名称（告警编码名称）
        /// </summary>
        public string Name { get; set; }


        [Required]
        /// <summary>
        /// 告警内容（原因）
        /// </summary>
        public string Content { get; set; }
    }
}
