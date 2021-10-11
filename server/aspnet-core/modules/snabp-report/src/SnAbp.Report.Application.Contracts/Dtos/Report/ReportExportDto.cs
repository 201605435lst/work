using SnAbp.Report.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Report.Dtos
{
    public class ReportExportDto : EntityDto<Guid>
    {
        public string KeyWords { get; set; }
        public List<Guid> Ids { get; set; }
        //查找的类型（接收：receive;汇报：send）
        public string ReportsType { get; set; }

        public ReportType Type { get; set; }
    }
}
