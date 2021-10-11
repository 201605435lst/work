/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileVersionDto
*******类 说 明： 文件版本dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/30 9:13:26
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using SnAbp.File.Entities;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileVersionDto:EntityDto<Guid>
    {
        /// <summary>
        ///     文件的版本
        /// </summary>
        public virtual int Version { get; set; }

        /// <summary>
        ///     对象存储服务的id
        /// </summary>
        public virtual Guid OssId { get; set; }

        /// <summary>
        ///     oss服务对象
        /// </summary>
        public virtual OssServer Oss { get; set; }

        /// <summary>
        ///     Oss 服务Url
        /// </summary>
        public virtual string OssUrl { get; set; }
    }
}
