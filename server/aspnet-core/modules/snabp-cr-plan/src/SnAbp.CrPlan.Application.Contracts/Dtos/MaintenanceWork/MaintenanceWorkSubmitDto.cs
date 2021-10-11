using SnAbp.File;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.CrPlan.Dtos.MaintenanceWork
{
    public class MaintenanceWorkSubmitDto
    {
        /// <summary>
        /// 维修作业id
        /// </summary>
        public Guid SkylightPlanId { get; set; }

        /// <summary>
        /// 维修作业关联的文件(方案)
        /// </summary>
        //public List<MaintenanceWorkRltFileCreateDto> MaintenanceWorkRltFiles { get; set; } = new List<MaintenanceWorkRltFileCreateDto>();
        public List<FileDomainDto> MaintenanceWorkRltFiles { get; set; }

        ///// <summary>
        ///// 封面文件
        ///// </summary>
        //public List<FileDomainDto> CoverFiles { get; set; } = new List<FileDomainDto>();
        ///// <summary>
        ///// 方案文件
        ///// </summary>
        //public List<FileDomainDto> ContentFiles { get; set; } = new List<FileDomainDto>();
    }
}
