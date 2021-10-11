

using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Dtos
{
    public class ReportRltUserSimpleDto : Entity<Guid>
    {
        /// <summary>
        /// 通知人员
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 报告
        /// </summary>
        public Guid ReportId { get; set; }
    }
}
