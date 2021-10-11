using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.CrPlan.Dto.RepairUser
{
    /// <summary>
    /// 设备的测试人员，添加/修改数据使用
    /// </summary>
    public class RepairUserCreateDto : Entity<Guid>, IRepairTagKeyDto
    {
        /// <summary>
        /// 作业人员
        /// </summary>
        public Guid UserId { get; set; }

        public string RepairTagKey { get ; set ; }
    }
}
