using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemRecordSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keyworlds { get; set; }
        /// <summary>
        /// 事件类型id
        /// </summary>
        public Guid EventTypeId { get; set; }
    }
}
