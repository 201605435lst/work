

using JetBrains.Annotations;
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleDto : EntityDto<Guid>
    {

        /// <summary>
        /// 组织机构id
        /// </summary>
        [NotNull] public Guid OrganizationId { get; set; }
        /// <summary>
        /// 组织机构
        /// </summary>
        [NotNull] public Organization Organization { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public List<DutyScheduleRltUserDto> DutyScheduleRltUsers { get; set; }

        /// <summary>
        /// 值班时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
