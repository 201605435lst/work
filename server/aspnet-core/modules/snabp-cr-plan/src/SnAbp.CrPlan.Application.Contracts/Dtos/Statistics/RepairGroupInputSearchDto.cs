using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class RepairGroupInputSearchDto : YearMonthPlanInputSearchDto
    {
        /// <summary>
        /// 设备类型分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrgId { get; set; }

    }
}
