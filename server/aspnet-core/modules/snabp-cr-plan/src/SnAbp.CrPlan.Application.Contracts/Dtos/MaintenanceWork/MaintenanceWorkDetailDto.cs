using SnAbp.CrPlan.Dto;
using SnAbp.CrPlan.Dtos.MaintenanceWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos
{
    public class MaintenanceWorkDetailDto
    {
        /// <summary>
        /// 维修计划关联的垂直天窗的工作票
        /// </summary>
        public List<MaintenanceWorkRltWorkTicketDto> MaintenanceWorkRltWorkTickets { get; set; } = new List<MaintenanceWorkRltWorkTicketDto>();

        /// <summary>
        /// 关联的文件
        /// </summary>
        public List<MaintenanceWorkRltFileSimpleDto> MaintenanceWorkRltFileSimples { get; set; } = new List<MaintenanceWorkRltFileSimpleDto>();

    }
}
