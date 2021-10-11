/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： FileVersion
*******类 说 明： 文件版本表，用来记录不同的文件历史版本
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 11:15:53
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SnAbp.File.Entities
{
    /// <summary>
    ///     文件版本表实体，继承 <see cref="CreationAuditedEntity" /> 类，实现了创建审计信息
    /// </summary>
    public class FileVersion : FullAuditedEntity<Guid>
    {
        public FileVersion()
        {
        }

        public FileVersion(Guid id)
        {
            Id = id;
        }

        public void SetId(Guid id)
        {
            this.Id = id;
        }
        /// <summary>
        ///     文件的Id
        /// </summary>
        public virtual Guid FileId { get; set; }

        /// <summary>
        ///     文件对象
        /// </summary>
        public virtual File File { get; set; }

        /// <summary>
        ///     文件的版本
        /// </summary>
        public virtual int Version { get; set; }

        /// <summary>
        ///     文件大小，通过字节数进行计算
        /// </summary>
        public virtual decimal Size { get; set; }
        /// <summary>
        ///     对象存储服务的id
        /// </summary>
        public virtual Guid OssId { get; set; }

        /// <summary>
        ///     oss服务对象
        /// </summary>
        public virtual OssServer Oss { get; set; }

        /// <summary>
        ///  Oss 服务Url
        /// </summary>
        public virtual string OssUrl { get; set; }

         public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}