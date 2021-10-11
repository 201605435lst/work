/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileVersionInputDto
*******类 说 明： 新增的文件版本数据实体
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/12 11:08:10
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileVersionCreateDto : EntityDto<Guid>
    {
        /// <summary>
        ///     文件名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        ///     原版本文件id
        /// </summary>
        [Required]
        public Guid FileId { get; set; }

        /// <summary>
        ///     文件大小，通过字节数进行计算
        /// </summary>
        [Required]
        public decimal Size { get; set; }

        /// <summary>
        ///     文件类型
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// 存储服务中的名称
        /// </summary>
        public string OssFileName { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; set; }
    }
}