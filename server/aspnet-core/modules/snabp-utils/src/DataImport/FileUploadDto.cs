/**********************************************************************
*******命名空间： SnAbp.Utils.DataImport
*******类 名 称： FileUploadDto
*******类 说 明： 文件上传dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/14 18:23:34
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
namespace SnAbp.Utils.DataImport
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
    }

    public class FileExportDto
    {
        /// <summary>
        /// 导出模板关键字
        /// </summary>
        public string TemplateKey { get; set; }

        /// <summary>
        /// 有效数字的起始行
        /// </summary>
        public int RowIndex { get; set; }

    }
}
