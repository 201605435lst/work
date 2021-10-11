/**********************************************************************
*******命名空间： SnAbp.Regulation.Entities
*******类 名 称： Institution
*******类 说 明： 制度表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/12/17 16:11:38
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Identity;
using SnAbp.MultiProject.MultiProject;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.Regulation.Entities
{
    public class Institution : AuditedEntity<Guid>
    {
        protected Institution() { }
        public Institution(Guid id) { Id = id; }

        ///<summary>
        ///项目id
        ///</summary>
         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }

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
        ///所属部门
        ///</summary>
        public virtual Guid? OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

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
        ///是否发布
        ///</summary>
        public bool IsPublish { get; set; }

        ///<summary>
        ///发布类别
        ///</summary>
        public int NewsClassify { get; set; }

        ///<summary>
        ///是否审批
        ///</summary>
        public bool IsApprove { get; set; }

        ///<summary>
        ///标签
        ///</summary>
        public List<InstitutionRltLabel> InstitutionRltLabels { get; set; }

        ///<summary>
        ///权限
        ///</summary>
        public List<InstitutionRltAuthority> InstitutionRltAuthorities { get; set; }

        ///<summary>
        ///文件附件
        ///</summary>
        public List<InstitutionRltFile> InstitutionRltFiles { get; set; }

        /// <summary>
        /// 关联版本
        /// </summary>
        public List<InstitutionRltEdition> InstitutionRltEditions { get; set; }

        /// <summary>
        /// 流程
        /// </summary>
        public InstitutionRltFlow InstitutionRltFlow { get; set; }

    }
}
