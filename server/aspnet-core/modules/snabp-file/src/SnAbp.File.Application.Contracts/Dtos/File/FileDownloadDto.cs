/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileFileDownloadDto
*******类 说 明： 文件下载传输对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/7/13 15:00:38
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SnAbp.File.Dtos
{
    public class FileDownloadDto:EntityDto<Guid>
    {
        /// <summary>
        /// 文件名称，通过文件所在文件夹的路径拼接：aaa文件/aa.txt
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///  文件大小
        /// </summary>
        public decimal Size { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 文件的路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>

        public string Type { get; set; }
    }
}
