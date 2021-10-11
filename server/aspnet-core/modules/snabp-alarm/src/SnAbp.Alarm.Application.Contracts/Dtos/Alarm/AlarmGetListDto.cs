using SnAbp.Alarm.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Alarm.Dtos
{
    public class AlarmGetListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string? Keywords { get; set; }


        /// <summary>
        /// 区域
        /// </summary>
        public string? ScopeCode { get; set; }


        /// <summary>
        /// 线路Id
        /// </summary>
        public Guid? RailwayId { get; set; }


        /// <summary>
        /// 车站Id
        /// </summary>
        public Guid? StationId { get; set; }


        /// <summary>
        /// 告警时间起点
        /// </summary>
        public DateTime? AlarmTimeStart { get; set; }


        /// <summary>
        /// 告警时间止点
        /// </summary>
        public DateTime? AlarmTimeEnd { get; set; }

        /// <summary>
        /// 系统Id（构建编码Id）
        /// </summary>
        public string? SystemCode { get; set; }


        /// <summary>
        /// 告警级别Id
        /// </summary>
        public AlarmLevel? Level { get; set; }
    }
}
