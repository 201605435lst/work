using SnAbp.Schedule.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ScheduleSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 专业Id
        /// </summary>
        public Guid? ProfessionId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public State? State { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 是否是前置任务选择框
        /// </summary>
        public Guid? NoId { get; set; }
    }
}
