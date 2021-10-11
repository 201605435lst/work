

using SnAbp.Identity;
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Dtos
{
    public class ReportRltUserDto : Entity<Guid>
    {
        /// <summary>
        /// 通知人员
        /// </summary>
        public virtual  Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        /// <summary>
        /// 报告
        /// </summary>
        public virtual Guid ReportId { get; set; }
        public virtual ReportDto Report { get; set; }
    }
}
