using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos
{
    public class WorkAttentionSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 数据是否是“类别”类型
        /// </summary>
        public bool IsType { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 是否树
        /// </summary>
        public bool isTree { get; set; }
        
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }
        /// <summary>
        /// 是否全查
        /// </summary>
        public bool IsAll { get; set; }
        public string RepairTagKey { get; set; }
    }
}
