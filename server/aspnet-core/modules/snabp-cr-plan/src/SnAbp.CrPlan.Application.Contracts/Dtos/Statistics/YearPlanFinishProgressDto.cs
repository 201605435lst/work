using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    /// <summary>
    /// 年表完成进度返回dto
    /// </summary>
    public class YearPlanFinishProgressDto : IRepairTagDto
    {
        public Guid OrgId { get; set; }
        public string OrgName { get; set; }

        /// <summary>
        /// 总体完成情况
        /// </summary>
        public FinishInfo TotalFinishInfo { get; set; } = new FinishInfo();

        /// <summary>
        /// 各设备完成情况
        /// </summary>
        public List<RepairGroupFinishInfo> RepairGroupFinishInfos { get; set; } = new List<RepairGroupFinishInfo>();

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
    public class FinishInfo : IRepairTagDto
    {
        public decimal FinishCount { get; set; }
        public decimal AlterCount { get; set; }
        public decimal TotalCount { get; set; }

        public decimal FinishPercent
        {
            get
            {
                if (TotalCount != 0)
                    return Math.Round((FinishCount / TotalCount) * 100, 2);
                else return 0;
            }
        }
        public decimal AlterPercent
        {
            get
            {
                if (TotalCount != 0)
                    return Math.Round((AlterCount / TotalCount) * 100, 2);
                else return 0;
            }
        }
        public Guid? RepairTagId { get; set; }

        public DataDictionaryDto RepairTag { get; set; }
    }

    public class RepairGroupFinishInfo : IRepairTagDto
    {
        public RepairGroupFinishInfo(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public FinishInfo FinishInfo { get; set; } = new FinishInfo();

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }
}
