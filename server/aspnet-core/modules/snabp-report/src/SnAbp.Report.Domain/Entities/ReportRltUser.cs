using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Entities
{
    public class ReportRltUser : Entity<Guid>
    {
        /// <summary>
        /// 通知人员
        /// </summary>
        public virtual Guid UserId { get; set; }
        public virtual IdentityUser User { get; set; }
        /// <summary>
        /// 报告
        /// </summary>
        public virtual Guid ReportId { get; set; }
        public virtual Report Report { get; set; }
        protected ReportRltUser() { }
        public ReportRltUser(Guid id)
        {
            Id = id;
        }
    }
}
