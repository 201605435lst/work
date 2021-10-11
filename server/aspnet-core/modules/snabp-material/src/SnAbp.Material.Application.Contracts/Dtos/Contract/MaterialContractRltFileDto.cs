/**********************************************************************
*******命名空间： SnAbp.Material.Dtos.Contract
*******类 名 称： ContractRltFileDto
*******类 说 明： 物资合同附件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:24:22
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.Material.Dtos
{
    /// <summary>
    /// 物资合同附件
    /// </summary>
    public class MaterialContractRltFileDto
    {
        public MaterialContractRltFileDto() { }
        public Guid FileId { get; set; }
        public File.Dtos.FileDto File { get; set; }
    }
}
