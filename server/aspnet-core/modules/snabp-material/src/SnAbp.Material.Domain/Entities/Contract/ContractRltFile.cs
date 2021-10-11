/**********************************************************************
*******命名空间： SnAbp.Material.Entities.Contract
*******类 名 称： ContractRltFile
*******类 说 明： 合同附件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:20:01
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 合同附件
    /// </summary>
    public class ContractRltFile : Entity<Guid>
    {
        public ContractRltFile(Guid contractId, Guid fileId)
        {
            this.ContractId = contractId;
            this.FileId = fileId;
        }

        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }
        public Guid FileId { get; set; }
        public File.Entities.File File { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { this.ContractId, this.FileId };
        }
    }
}
