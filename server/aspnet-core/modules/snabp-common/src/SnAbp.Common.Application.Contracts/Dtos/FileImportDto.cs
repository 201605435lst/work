/**********************************************************************
*******命名空间： SnAbp.Common.Dtos
*******类 名 称： FileImportDto
*******类 说 明： 文件导入dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 10:46:24
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SnAbp.Common.Dtos
{
    public class FileImportDto
    {
        /// <summary>
        /// 缓存key
        /// </summary>
        public string CacheKey { get; set; }

        /// <summary>
        /// 进度数据
        /// </summary>
        public decimal Progress { get; set; }

        /// <summary>
        /// 文件导入完成后清除缓存的时间间隔，默认10s
        /// </summary>
        public int ExpirationTime { get; set; } = 10;

        /// <summary>
        /// 缓存更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 导入工作是否执行完成
        /// </summary>
        public bool Success { get; set; }
    }

    public class FileExportDto
    {
        public  string FileName { get; set; }

        public Stream Stream { get; set; }
    }

    public class FileCacheModal
    {
        public string FileName { get; set; }

        public byte[] FileBytes { get; set; }
    }
}
