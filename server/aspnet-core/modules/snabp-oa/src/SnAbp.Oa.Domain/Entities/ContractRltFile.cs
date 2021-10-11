/**********************************************************************
*******命名空间： SnAbp.Oa.Entities
*******类 名 称： ContractRltFile
*******类 说 明： 合同关联文件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 11:49:55
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities;

namespace SnAbp.Oa.Entities
{
    public sealed class ContractRltFile : Entity<Guid>
    {
        public ContractRltFile(Guid id) => Id = id;
        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }

        public Guid FileId { get; set; }
        public File.Entities.File File { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

    }
}
