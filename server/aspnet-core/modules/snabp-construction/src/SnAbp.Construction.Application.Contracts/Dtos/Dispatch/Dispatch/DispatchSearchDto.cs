using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Construction.Dtos
{
    /// <summary>
    /// 派工单
    /// </summary>
    public class DispatchSearchDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string Keyword { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否查询审批数据（根据此字段过滤）
        /// </summary>
        public bool Approval { get; set; }
        /// <summary>
        /// 是否获取待我审批的数据
        /// </summary>
        public bool Waiting { get; set; }
        /// <summary>
        /// 是否获取已审批的数据 
        /// </summary>
        public bool Passed { get; set; }
        /// <summary>
        /// 是否获取施工日志填报的数据
        /// </summary>
        public bool IsForDaily { get; set; }
        /// <summary>
        /// 是否分页
        /// </summary>
        public bool IsAll { get; set; }
    }
}
