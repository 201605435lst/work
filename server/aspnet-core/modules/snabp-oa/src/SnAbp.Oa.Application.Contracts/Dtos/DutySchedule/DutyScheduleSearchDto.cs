using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class DutyScheduleSearchDto:EntityDto<Guid>
    {
        /// <summary>
        /// 组织机构id
        /// </summary>
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 人员
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// 值班时间
        /// </summary>
        public string Date { get; set; }
    }
}
