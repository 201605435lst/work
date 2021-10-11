using SnAbp.StdBasic.Enums;
using System;
using System.Collections.Generic;
using SnAbp.File.Dtos;
using SnAbp.CrPlan.Dtos;
using SnAbp.Identity;

namespace SnAbp.CrPlan.Dto.MaintenanceRecord
{
    public class MaintenanceRecordDto : IRepairTagDto
    {
        /// <summary>
        /// 测试时间
        /// </summary>
        public DateTime WorkOrderRealEndTime { get; set; }

        /// <summary>
        /// 测试记录
        /// </summary>
        public List<MaintenanceRepairGroup> RecordDatas { get; set; } = new List<MaintenanceRepairGroup>();
        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }
    }

    public class MaintenanceRepairGroup
    {
        public Guid RepairItemId { get; set; }

        /// <summary>
        /// 测试项所属维修项
        /// </summary>
        public string RepairItem { get; set; }

        /// <summary>
        /// 测试项内容集合
        /// </summary>
        public List<MaintenanceRecordTestItem> TestItems { get; set; } = new List<MaintenanceRecordTestItem>();

        /// <summary>
        /// 检修人
        /// </summary>
        public string TestUserName { get; set; }

        /// <summary>
        /// 验收人
        /// </summary>
        public string CheckUserName { get; set; }
    }

    public class MaintenanceRecordTestItem
    {
        /// <summary>
        /// 测试项
        /// </summary>
        public string RepairTestItem { get; set; }

        /// <summary>
        /// 测试类型
        /// </summary>
        public RepairTestType? TestType { get; set; }

        /// <summary>
        /// 检修结果
        /// </summary>
        public string TestResult { get; set; }

        /// <summary>
        /// 检修文件id
        /// </summary>
        public Guid? FileId { get; set; }
        /// <summary>
        /// 检修文件
        /// </summary>
        public FileSimpleDto File { get; set; }

        /// <summary>
        /// 验收结果
        /// </summary>
        public string CheckResult { get; set; }

        /// <summary>
        /// 验收是否合格  1合格 2不合格 3其他
        /// </summary>
        public int IsQualified
        {
            get
            {
                if (CheckResult == "合格") return 1;
                else if (CheckResult == "不合格") return 2;
                else return 3;
            }
        }
    }
}
