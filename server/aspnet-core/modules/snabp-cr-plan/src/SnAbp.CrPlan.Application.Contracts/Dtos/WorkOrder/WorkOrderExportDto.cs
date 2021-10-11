using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dto.WorkOrder
{
    /// <summary>
    /// 派工单导出Dto
    /// </summary>
    public class WorkOrderExportDto
    {
        /// <summary>
        /// 车间
        /// </summary>
        public string WorkShop { get; set; }
        /// <summary>
        /// 工区
        /// </summary>
        public string WorkArea { get; set; }
        /// <summary>
        /// 命令票号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 作业组长
        /// </summary>
        public string WorkLeader { get; set; }
        /// <summary>
        /// 成员
        /// </summary>
        public string WorkMemberList { get; set; }
        /// <summary>
        /// 工作地点内容及影响
        /// </summary>
        public string WorkSiteAndContent { get; set; }
        /// <summary>
        /// 驻站联络员
        /// </summary>
        public string StationLiaisonOfficerList { get; set; }
        /// <summary>
        /// 现场防护员
        /// </summary>
        public string FieldGuardList { get; set; }
        /// <summary>
        /// 通信工具检查实验情况
        /// </summary>
        public string ToolSituation { get; set; }
        
        /// <summary>
        /// 工区工长
        /// </summary>
        public string WorkAreaLeader { get; set; }
        /// <summary>
        /// 作业时间
        /// </summary>
        public string WorkTime { get; set; }
        /// <summary>
        /// 天窗时间
        /// </summary>
        public string SkylightTime { get; set; }
        /// <summary>
        /// 完成情况
        /// </summary>
        public string CompletionStatus { get; set; }
        public WorkOrderExportDto() { }
    }
}
