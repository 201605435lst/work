using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.StdBasic.Dtos.Repair.RepairItem
{
    public class RepairItemUpdateSimpleDto
    {
        /// <summary>
        /// 维修项ids
        /// </summary>
        public List<Guid> RepairItemIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 执行单位ids
        /// </summary>
        public List<Guid> OrganizationTypeIds { get; set; } = new List<Guid>();
    }
}
