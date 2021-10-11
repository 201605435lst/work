using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Message.Bpm.Dtos
{
    public class BpmMessageSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字 设备名称 设备编码
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool? IsProcess { get; set; }
    }
}
