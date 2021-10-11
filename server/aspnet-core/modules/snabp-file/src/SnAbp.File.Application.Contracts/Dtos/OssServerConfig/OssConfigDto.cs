/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： OssConfigDto
*******类 说 明： 对象存储配置添加成功后返回对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 14:30:32
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class OssConfigDto : EntityDto<Guid>
    {
        /// <summary>
        ///     服务名称
        /// </summary>
        public virtual string Name { get; set; }

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
        ///     备用字段
        /// </summary>
        public virtual string Extend { get; set; }

        /// <summary>
        /// 文件数量 TODO 计算文件数量
        /// </summary>
        public  int Count { get; set; }
    }
}