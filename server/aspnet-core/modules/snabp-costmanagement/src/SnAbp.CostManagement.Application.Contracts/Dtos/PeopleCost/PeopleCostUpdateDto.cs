using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CostManagement.Dtos
{
    public class PeopleCostUpdateDto : EntityDto<Guid>
    {

        /// <summary>
        /// 专业
        /// </summary>
        public virtual Guid ProfessionalId { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 收款单位
        /// </summary>
        public virtual Guid PayeeId { get; set; }
    }
}
