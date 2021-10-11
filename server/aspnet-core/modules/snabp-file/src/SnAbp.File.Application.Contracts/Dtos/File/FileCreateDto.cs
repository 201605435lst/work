/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileInputDto
*******类 说 明： 文件输入dto,用于前端输入参数
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/12 9:21:47
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.ComponentModel.DataAnnotations;

namespace SnAbp.File.Dtos
{
    public class FileCreateDto
    {
        [Required] public Guid FileId { get; set; }

        [Required] public string Name { get; set; }


        /// <summary>
        /// 文件类型，存储文件的后缀名
        /// </summary>
        [Required] public string Type { get; set; }


        /// <summary>
        /// 文件大小，通过字节数进行计算
        /// </summary>
        [Required] public decimal Size { get; set; }

        /// <summary>
        ///     是否公开，需要用户选择配置，默认是公开的
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        ///     租户Id
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        ///     组织Id
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        ///     隶属文件夹的Id
        /// </summary>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// 文件夹路径，上传整个文件夹时需要使用
        /// </summary>
        public string FolderPath { get; set; }
        /// <summary>
        /// 存储服务中的名称
        /// </summary>
        public string OssFileName { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// 静态文件夹key
        /// </summary>
        public string StaticKey { get; set; }
    }
}