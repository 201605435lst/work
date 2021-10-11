
using System;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Report.Dtos
{
   public  class ReportRltFileSimpleDto:Entity<Guid>
    {
        /// <summary>
        /// 工作汇报
        /// </summary>
        public Guid ReportId { get; set; }
      
        /// <summary>
        /// 文件
        /// </summary>
        public Guid FileId { get; set; }
      

    }
}
