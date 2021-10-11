using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using SnAbp.Identity;
using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CrPlan.Dto.Worker
{
    /// <summary>
    /// 派工单作业人员，获取数据使用
    /// </summary>
    public class WorkerDto : EntityDto<Guid>, IRepairTagDto
    {
        /// <summary>
        /// 派工单ID
        /// </summary>
        public Guid WorkOrderId { get; set; }

        /// <summary>
        /// 作业人员
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 作业人员名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 人员职责
        /// 0-作业成员，1-作业组长，2-现场防护员，3-驻站联络员
        /// </summary>
        public UserDuty Duty { get; set; }

        public Guid? RepairTagId { get; set; }
        public DataDictionaryDto RepairTag { get; set; }

        public WorkerDto() { }
        public WorkerDto(Guid id) { Id = id; }
    }
}
