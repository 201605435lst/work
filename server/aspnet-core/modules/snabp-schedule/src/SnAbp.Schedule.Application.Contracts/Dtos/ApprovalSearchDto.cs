using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Schedule.Dtos
{
    public class ApprovalSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 专业
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
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 是否我填报的
        /// </summary>
        public bool IsCreator { get; set; }

        /// <summary>
        /// 我发起的
        /// </summary>
        public bool IsInitiate { get; set; }

        /// <summary>
        /// 我接收的
        /// </summary>
        public bool IsCC { get; set; }
    }
}
