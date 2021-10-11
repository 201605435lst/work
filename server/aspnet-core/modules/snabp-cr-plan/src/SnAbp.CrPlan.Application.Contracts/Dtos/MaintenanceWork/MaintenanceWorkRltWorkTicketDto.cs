using SnAbp.CrPlan.Dtos;
using SnAbp.File;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto
{
    public class MaintenanceWorkRltWorkTicketDto : EntityDto<Guid>
    {
        public string PlanIndex { get; set; }
        public string Railway { get; set; }
        public string Level { get; set; }
        public string RailwayType { get; set; }
        public string MaintenanceProject { get; set; }
        public string MaintenanceType { get; set; }
        public string MaintenanceLocation { get; set; }
        public string RepaireDate { get; set; }
        public string RepaireTime { get; set; }
        public string Incidence { get; set; }
        public string TrainInfo { get; set; }
        public string WorkOrgAndDutyPerson { get; set; }
        public string CooperationUnit { get; set; }
        public string SignOrganization { get; set; }
        public string FirstTrial { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 关联工作票
        /// </summary>
        public List<WorkTicketDto> WorkTickets { get; set; } = new List<WorkTicketDto>();

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime PlanStartTime { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime PlanEndTime { get; set; }
    }
}
