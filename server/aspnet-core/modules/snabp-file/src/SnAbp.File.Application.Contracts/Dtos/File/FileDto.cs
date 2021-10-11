/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileUploadDto
*******类 说 明： 文件上传dto,包含签名信息和oss中存储的文件名
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/12 10:11:57
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.ComponentModel.DataAnnotations.Schema;
using SnAbp.File.OssSdk;

namespace SnAbp.File.Dtos
{
    [NotMapped]
    public class FileDto
    {
        public Guid FileId { get; set; }
        public string PresignUrl { get; set; }
        public string OssFileName { get; set; }
        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// oss 服务类型，根据不同类型调用不同的文件上传方法
        /// </summary>
        public string OssType { get; set; }
    }
}