using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleCreateDto
    {
        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public List<DutyScheduleRltUserCreateDto> DutyScheduleRltUsers { get; set; }

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
