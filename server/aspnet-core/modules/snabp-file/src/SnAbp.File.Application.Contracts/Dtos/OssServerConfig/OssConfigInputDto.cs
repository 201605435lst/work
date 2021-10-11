/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： OssServerConfigInputDto
*******类 说 明： 对象存储服务配置表单输入对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 14:14:51
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class OssConfigInputDto : EntityDto<Guid>
    {

        /// <summary>
        /// 服务名称，minio\阿里云oss\亚马逊s3
        /// </summary>
        [Required] public virtual string Name { get; set; }



        /// <summary>
        /// 存储服务类型
        /// </summary>
        [Required] public virtual string Type { get; set; }


        /// <summary>
        /// 服务地址
        /// </summary>
        [Required] public virtual string EndPoint { get; set; }


        /// <summary>
        /// 身份id
        /// </summary>
        [Required] public virtual string AccessKey { get; set; }


        /// <summary>
        /// 访问密钥
        /// </summary>
        [Required] public virtual string AccessSecret { get; set; }
    }
}