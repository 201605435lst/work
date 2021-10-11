using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Entities
{
   public class ReportRltFile : Entity<Guid>
    {
        /// <summary>
        /// 工作汇报
        /// </summary>
        public virtual Guid ReportId { get; set; }
        public virtual Report Report { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

        protected ReportRltFile() { }
        public ReportRltFile(Guid id)
        {
            Id = id;
        }
    }
}