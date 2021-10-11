using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CostManagement.Dtos
{
    public class PeopleCostSearchDto : PagedAndSortedResultRequestDto
    {

        /// <summary>
        /// 专业
        /// </summary>
        public virtual Guid? ProfessionalId { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public virtual Guid? PayeeId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
