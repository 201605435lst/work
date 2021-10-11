/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileInputDto
*******类 说 明： 文件签名获取传输对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/7/9 10:40:14
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace SnAbp.File.Dtos
{
    public class FileInputDto:EntityDto
    {
        /// <summary>
        ///  文件后缀名
        /// </summary>
        [Required]public  string Sufixx { get; set; }

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string BucketName { get; set; }
    }
}
