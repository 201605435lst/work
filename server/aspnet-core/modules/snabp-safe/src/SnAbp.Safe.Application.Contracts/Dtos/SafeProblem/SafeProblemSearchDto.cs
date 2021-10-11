using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Safe.Dtos
{
    public class SafeProblemSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 标题查询
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 问题类型
        /// </summary>
        public virtual Guid? TypeId { get; set; }

        /// <summary>
        /// 风险等级
        /// </summary>
        public SafetyRiskLevel? RiskLevel { get; set; }

        /// <summary>
        /// 标题查询
        /// </summary>
        public SafeFilterType FilterType { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否全部显示
        /// </summary>
        public bool IsAll { get; set; }
        
        /// <summary>
        /// 只查询已整改的
        /// </summary>
        public bool IsSelect { get; set; }

    }
}
