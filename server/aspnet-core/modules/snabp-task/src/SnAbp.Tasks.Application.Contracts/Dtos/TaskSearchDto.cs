using SnAbp.Tasks.enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Tasks.Dtos
{
    public class TaskSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 任务分组
        /// </summary>
        public TaskGroup Group { get; set; } = TaskGroup.All;

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 是否有任务
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
