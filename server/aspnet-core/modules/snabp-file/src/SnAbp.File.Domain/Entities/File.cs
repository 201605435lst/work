/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： File
*******类 说 明： 文件表实体类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/8 11:37:50
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using SnAbp.Identity;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.File.Entities
{
    /// <summary>
    ///     文件实体，继承自<see cref="FullAuditedEntity" /> ,实现了全审计，包含创建、删除、和修改等审计记录。
    /// </summary>
    public class File : FullAuditedEntity<Guid>
    {
        public File()
        {
        }

        public void SetId(Guid id)
        {
            Id = id;
        }
        public File(Guid id)
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
        ///     文件夹对象
        /// </summary>
        public virtual Folder? Folder { get; set; }

        /// <summary>
        ///     隶属文件夹的Id
        /// </summary>
        public virtual Guid? FolderId { get; set; }
        /// <summary>
        ///     文件名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     文件类型，存储文件的后缀名
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        ///     文件大小，通过字节数进行计算
        /// </summary>
        public virtual decimal Size { get; set; }

        /// <summary>
        ///     文件是否分享，共享
        /// </summary>
        public virtual bool IsShare { get; set; }

        /// <summary>
        ///     是否隐藏，在回收站删除文件时，如果数据有被关联，则改变该状态
        /// </summary>
        public virtual bool IsHidden { get; set; }

        /// <summary>
        ///     是否公开，文件第一次上传时状态为false,当上传者修改时才改变该文件的状态到指定的组织结构
        /// </summary>
        public virtual bool IsPublic { get; set; }


        /// <summary>
        ///     存储路径，用于文件删除后恢复文件使用<code></code>
        ///     例如：路径格式：文件夹1/文件夹2/文件夹3/...
        /// </summary>
        public virtual string Path { get; set; }

        /// <summary>
        ///     文件相对地址，由存储服务器返回的永久访问地址
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 文件夹标签
        /// </summary>
        public virtual List<FileRltTag> Tags { get; set; }

        /// <summary>
        /// 文件版本集合
        /// </summary>
        public virtual List<FileVersion> Versions { get; set; }

        /// <summary>
        /// 文件共享的信息
        /// </summary>
        public virtual List<FileRltShare> Shares { get; set; }

        /// <summary>
        /// 文件的权限
        /// </summary>
        public virtual List<FileRltPermissions> Permissions { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}