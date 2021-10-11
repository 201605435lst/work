using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.CostManagement.Dtos
{
    public class CostContractDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 合同名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        ///合同类型
        /// </summary>
        public virtual Guid TypeId { get; set; }
        public virtual DataDictionary Type { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? Date { get; set; }
        
        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? CreatorName { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public List<CostContractRltFileDto> ContractRltFiles { get; set; } = new List<CostContractRltFileDto>();
    }
}
