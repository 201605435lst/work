using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CostManagement.Dtos
{
    public class CostContractRltFileSimpleDto : EntityDto<Guid>
    {
        /// <summary>
        /// 合同
        /// </summary>
        public virtual Guid ContractId { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
    }
}
