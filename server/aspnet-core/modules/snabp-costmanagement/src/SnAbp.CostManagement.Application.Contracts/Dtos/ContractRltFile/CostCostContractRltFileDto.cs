using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CostManagement.Dtos
{
   public class CostContractRltFileDto : EntityDto<Guid>
    {
        /// <summary>
        /// 合同
        /// </summary>
        public virtual Guid ContractId { get; set; }
        public virtual CostContractDto Contract { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public virtual Guid FileId { get; set; }
        public virtual File.Entities.File File { get; set; }
    }
}
