using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CostManagement.Dtos
{
    public class  MoneyListCreateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 资金类别
        /// </summary>
        public virtual Guid TypeId { get; set; }
        /// <summary>
        /// 应收
        /// </summary>
        public decimal Receivable { get; set; }
        /// <summary>
        /// 已收
        /// </summary>
        public decimal Received { get; set; }
        /// <summary>
        /// 应付
        /// </summary>
        public decimal Due { get; set; }
        /// <summary>
        /// 已付
        /// </summary>
        public decimal Paid { get; set; }
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
