/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Contract
*******类 名 称： ContractDto
*******类 说 明： 物资合同
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:23:31
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Application.Dtos;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 物资合同
    /// </summary>
    public class MaterialContractDto:EntityDto<Guid>
    {
        public MaterialContractDto() { }

        /// <summary>
        /// 合同编号
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///  合同日期
        /// </summary>
        public virtual DateTime Date { get; set; }
        /// <summary>
        /// 合同金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 合同附件
        /// </summary>
        public List<MaterialContractRltFileDto> Files { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public Identity.IdentityUserDto Creator { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
