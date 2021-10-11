/**********************************************************************
*******命名空间： SnAbp.File
*******类 名 称： FileDomainDto
*******类 说 明： 领域层文件dto
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/10/22 16:32:46
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File
{
    public class FileDomainDto
    {
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size { get; set; }
    }
}
