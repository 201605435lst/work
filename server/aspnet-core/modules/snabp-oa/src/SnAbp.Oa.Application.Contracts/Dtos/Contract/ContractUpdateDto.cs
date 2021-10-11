using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.Oa.Dtos
{
    public class ContractUpdateDto : EntityDto<Guid>
    {
        /// <summary>
        /// 合同名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 甲方
        /// </summary>
        public string PartyA { get; set; }
        /// <summary>
        /// 乙方
        /// </summary>
        public string PartyB { get; set; }
        /// <summary>
        /// 丙方
        /// </summary>
        public string PartyC { get; set; }

        /// <summary>
        /// 合同签订时间
        /// </summary>
        public DateTime SignTime { get; set; }
        /// <summary>
        /// 主办部门id
        /// </summary>
        public Guid HostDepartmentId { get; set; }
        ///// <summary>
        ///// 主办部门
        ///// </summary>
        //public Organization HostDepartment { get; set; }

        /// <summary>
        /// 承办人id
        /// </summary>
        public Guid UndertakerId { get; set; }
        ///// <summary>
        ///// 承办人id
        ///// </summary>
        //public IdentityUser Undertaker { get; set; }
        /// <summary>
        /// 承办部门id
        /// </summary>
        public Guid? UnderDepartmentId { get; set; }
        ///// <summary>
        ///// 承办部门
        ///// </summary>
        //public Organization UnderDepartment { get; set; }

        /// <summary>
        /// 合同总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 金额大写
        /// </summary>
        public string AmountWords { get; set; }

        /// <summary>
        /// 预算
        /// </summary>
        public decimal Budge { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public Guid? TypeId { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 对方信息
        /// </summary>
        public string OtherPartInfo { get; set; }

        /// <summary>
        /// 合同附件
        /// </summary>
        public List<ContractRltFileDto> ContractRltFiles { get; set; } = new List<ContractRltFileDto>();
    }
}
