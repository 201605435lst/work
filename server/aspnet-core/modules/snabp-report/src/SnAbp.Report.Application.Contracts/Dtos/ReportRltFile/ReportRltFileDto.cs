
using System;
using System.IO;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Dtos
{
    public class ReportRltFileDto:Entity<Guid>
    {

        /// <summary>
        /// 工作汇报
        /// </summary>
        public virtual Guid ReportId { get; set; }
        public virtual ReportDto Report { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }

    }
}
