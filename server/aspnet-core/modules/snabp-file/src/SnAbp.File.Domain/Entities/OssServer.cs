/**********************************************************************
*******命名空间： SnAbp.File.Entities
*******类 名 称： OssServerConfig
*******类 说 明： 对象存储服务配置表
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/8 14:18:25
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Entities
{
    public class OssServer : Entity<Guid>
    {
        public OssServer()
        {
        }

        public OssServer(Guid id)
        {
            Id = id;
        }

        /// <summary>
        ///     服务名称，指具体的oss 服务配置的名称，定义后不可修改
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///     连接名，oss服务配置的连接名，用来在数据迁移时过滤该服务下的文件
        /// </summary>
        public virtual string ConnName { get; set; }

        /// <summary>
        ///     服务源类型，minio\阿里云oss\亚马逊s3
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        ///     启用当前服务
        /// </summary>
        public virtual bool Enable { get; set; }

        /// <summary>
        ///     服务地址
        /// </summary>
        public virtual string EndPoint { get; set; }

        /// <summary>
        ///     服务状态
        /// </summary>
        public virtual string State { get; set; }

        /// <summary>
        ///     身份id
        /// </summary>
        public virtual string AccessKey { get; set; }

        /// <summary>
        ///     访问密钥
        /// </summary>
        public virtual string AccessSecret { get; set; }

        /// <summary>
        /// 当前存储服务中的文件对象集
        /// </summary>
        public virtual List<FileVersion> FileVersions { get; set; }

        public Guid? ProjectTagId { get; set; }
        public Guid? OrganizationRootTagId { get; set; }
    }
}