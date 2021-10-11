
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleUpdateDto:EntityDto<Guid>
    {
        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public List<DutyScheduleRltUserUpdateDto> DutyScheduleRltUsers { get; set; }

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
