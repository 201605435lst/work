/**********************************************************************
*******命名空间： SnAbp.Oa.Entities
*******类 名 称： Contract
*******类 说 明： 合同表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/1 10:19:50
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Oa.Entities
{
    public class Contract : FullAuditedEntity<Guid>
    {
        public Contract(Guid id) => Id = id;
        public Contract() { }
        public void SetId(Guid id)
        {
            Id = id;
        }
        /// <summary>
        /// 合同名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// 甲方
        /// </summary>
        public virtual string PartyA { get; set; }
        /// <summary>
        /// 乙方
        /// </summary>
        public virtual string PartyB { get; set; }
        /// <summary>
        /// 丙方
        /// </summary>
        public virtual string PartyC { get; set; }

        /// <summary>
        /// 合同签订时间
        /// </summary>
        public virtual DateTime SignTime { get; set; }
        /// <summary>
        /// 主办部门id
        /// </summary>
        public virtual Guid HostDepartmentId { get; set; }
        /// <summary>
        /// 主办部门
        /// </summary>
        public virtual Organization HostDepartment { get; set; }

        /// <summary>
        /// 承办人id
        /// </summary>
        public virtual Guid UndertakerId { get; set; }
        /// <summary>
        /// 承办人id
        /// </summary>
        public virtual IdentityUser Undertaker { get; set; }
        /// <summary>
        /// 承办部门id
        /// </summary>
        public virtual Guid? UnderDepartmentId { get; set; }
        /// <summary>
        /// 承办部门
        /// </summary>
        public virtual Organization UnderDepartment { get; set; }

        /// <summary>
        /// 合同总金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 金额大写
        /// </summary>
        public virtual string AmountWords { get; set; }

        /// <summary>
        /// 预算
        /// </summary>
        public virtual decimal Budge { get; set; }
        /// <summary>
        /// 合同类型
        /// </summary>
        public virtual Guid? TypeId { get; set; }
        public virtual DataDictionary Type { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Abstract { get; set; }

        /// <summary>
        /// 对方信息
        /// </summary>
        public virtual string OtherPartInfo { get; set; }

        /// <summary>
        /// 合同附件
        /// </summary>
        public List<ContractRltFile> ContractRltFiles { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

        // 关联文档暂留
    }
}
