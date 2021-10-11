/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Contract
*******类 名 称： ContractCreateDto
*******类 说 明： 物资合同创建实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:27:49
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
    /// 物资合同创建实体
    /// </summary>
    public class MaterialContractCreateDto:EntityDto<Guid>
    {
        public string Name { get; set; }       
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public List<Guid> FileIds { get; set; }
    }
}
