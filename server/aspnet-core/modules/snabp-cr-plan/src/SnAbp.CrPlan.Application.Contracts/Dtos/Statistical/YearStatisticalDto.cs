using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.Statistical
{
    /// <summary>
    /// 年表统计实体
    /// </summary>
    public class YearStatisticalDto : IRepairTagDto
    {
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string OrganizationName { get; set; }


        /// <summary>
        /// 完成百分比
        /// </summary>
        public decimal Percentage { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
