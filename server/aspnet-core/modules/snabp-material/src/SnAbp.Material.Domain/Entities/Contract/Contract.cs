/**********************************************************************
*******命名空间： SnAbp.Material.Entities.Contract
*******类 名 称： Contract
*******类 说 明： 物资合同表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/10 9:13:45
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.MultiProject.MultiProject;

using System;
using System.Collections.Generic;
using System.Text;

using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Material.Entities
{
    /// <summary>
    /// 物资合同表
    /// </summary>
    public class Contract : AuditedEntity<Guid>
    {
        public Contract(Guid id) => this.Id = id;
        public void SetId(Guid id) => this.Id = id;

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
        public List<ContractRltFile> Files { get; set; }

        /// <summary>
        /// 合同上传人
        /// </summary>
        public Identity.IdentityUser Creator { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}
