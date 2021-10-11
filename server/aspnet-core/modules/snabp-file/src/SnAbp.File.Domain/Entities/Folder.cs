/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： Folder
*******类 说 明： 文件夹表实体类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/8 11:10:46
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using SnAbp.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.File.Entities
{
    /// <summary>
    ///     文件夹实体，继承自 <see cref="FullAuditedEntity" /> 实现了全审计字段
    /// </summary>
    public class Folder : FullAuditedEntity<Guid>
    {
        public Folder()
        {
        }

        public void SetId(Guid id)
        {
            this.Id = id;
        }
        public Folder(Guid id)
        {
            Id = id;
        }

        /// <summary>
        ///     租户Id
        /// </summary>
        public virtual Guid? TenantId { get; set; }

        /// <summary>
        ///     组织Id
        /// </summary>
        public virtual Guid? OrganizationId { get; set; }

        /// <summary>
        ///     父节点的Id
        /// </summary>
        public virtual Guid? ParentId { get; set; }
        public virtual Folder Parent { get; set; }
        /// <summary>
        ///     文件夹名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        ///     是否被分享
        /// </summary>
        public virtual bool IsShare { get; set; }

        /// <summary>
        ///     是否公开
        /// </summary>
        public virtual bool IsPublic { get; set; }

        /// <summary>
        ///     指定key的文件夹
        /// </summary>
        public string StaticKey { get; set; }

        /// <summary>
        ///     文件夹路径
        ///     路径格式：文件夹1/文件夹2/文件夹3/..
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        /// 当前文件的权限信息
        /// </summary>
        public virtual List<FolderRltPermissions> Permissions { get; set; }

        /// <summary>
        /// 文件夹共享新
        /// </summary>
        public virtual List<FolderRltShare> Shares { get; set; }

        /// <summary>
        /// 文件夹标签
        /// </summary>
        public virtual List<FolderRltTag> Tags { get; set; }

        /// <summary>
        /// 子文件
        /// </summary>
        public virtual List<File> Files { get; set; }

        [NotMapped]
        public virtual Guid? FolderId { get; set; }
        /// <summary>
        /// 子文件夹
        /// </summary>
        public virtual List<Folder> Folders { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}