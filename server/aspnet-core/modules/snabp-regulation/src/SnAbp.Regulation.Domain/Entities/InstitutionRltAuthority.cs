/**********************************************************************
*******命名空间： SnAbp.Regulation.Entities
*******类 名 称： InstitutionRltAuthority
*******类 说 明： 制度权限关联表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/17 16:12:27
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace SnAbp.Regulation.Entities
{
    public class InstitutionRltAuthority:Entity<Guid>
    {
        protected InstitutionRltAuthority() { }
        public InstitutionRltAuthority(Guid id) { Id = id; }

        public virtual Guid InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        /// <summary>
        ///成员Id，对应某个指定的组织，角色或者用户
        /// </summary>
        public virtual Guid MemberId { get; set; }

        /// <summary>
        /// 成员类型，对应组织，角色，用户
        /// </summary>
        public virtual MemberType Type { get; set; }

        ///<summary>
        ///是否可查看
        ///</summary>
        public bool IsView { get; set; }

        ///<summary>
        ///是否可编辑
        ///</summary>>
        public bool IsEdit { get; set; }

        ///<summary>
        ///可下载者
        ///</summary>
        public bool IsDownLoad { get; set; }



    }
}
