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

using Microsoft.AspNetCore.Http;

namespace SnAbp.File.Dtos
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
    }
}