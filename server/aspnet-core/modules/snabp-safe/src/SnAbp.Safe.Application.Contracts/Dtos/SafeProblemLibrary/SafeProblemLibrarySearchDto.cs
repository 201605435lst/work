using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
   public class SafeProblemLibrarySearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 事件类型
        /// </summary>
        public Guid? EventTypeId { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        public SafetyRiskLevel? RiskLevel { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }
    }
}
