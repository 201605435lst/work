/**********************************************************************
*******命名空间： SnAbp.Regulation.Entities
*******类 名 称： InstitutionRltEdition
*******类 说 明： 制度版本关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/17 16:13:45
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Regulation.Entities
{
    public class InstitutionRltEdition : AuditedEntity<Guid>
    {
        protected InstitutionRltEdition() { }
        public InstitutionRltEdition(Guid id) { Id = id; }

        public virtual Guid InstitutionId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }

        ///<summary>
        ///状态
        ///</summary>
        public string State { get; set; }

        ///<summary>
        ///制度分类
        ///</summary>
        public int Classify { get; set; }

        ///<summary>
        ///生效时间
        ///</summary>
        public DateTime EffectiveTime { get; set; }

        ///<summary>
        ///过期时间
        ///</summary>
        public DateTime ExpireTime { get; set; }

        ///<summary>
        ///摘要
        ///</summary>
        public string Abstract { get; set; }
        
        ///<summary>
        ///所属部门
        ///</summary>
        public virtual Guid? OrganizationId { get; set; }

        ///<summary>
        ///版本
        ///</summary>
        public decimal Version { get; set; }
    }
}
