using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.Text;
namespace SnAbp.Regulation.Dtos.Institution
{
    public class InstitutionRltAuthorityDto
    {
        /// <summary>
        ///成员Id，对应某个指定的组织，角色或者用户
        /// </summary>
        public virtual Guid? MemberId { get; set; }

        /// <summary>
        /// 制度Id
        /// </summary>
        public virtual Guid? InstitutionId { get; set; }

        /// <summary>
        /// 成员类型，对应组织，角色，用户
       // </summary>
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
