using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto
{
    /// <summary>
    /// 导出EXCEL
    /// </summary>
    public class YearMonthExportDto: IRepairTagKeyDto
    {
        /// <summary>
        /// 生成年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 生成月分
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 生成车间
        /// </summary>
        public Guid OrgId { get; set; }

        /// <summary>
        /// 生成类型(年表,月表,年度月表)
        /// </summary>
        public int PlanType { get; set; }

        public string RepairTagKey { get ; set ; }

        /// <summary>
        /// 新增需求：添加年月表变更记录表
        /// </summary>
        public Guid? AlterRecordId { get; set; }

        public bool IsCreateRecord { get; set; }

    }
}
