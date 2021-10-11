/**********************************************************************
*******命名空间： SnAbp.File.Dtos
*******类 名 称： FileMigrationDto
*******类 说 明： 文件迁移返回数据对象
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 13:41:43
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File.Dtos
{
    public class FileMigrationDto
    {
        /// <summary>
        /// 是否迁移成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 迁移进度信息
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 迁移文件的总条数
        /// </summary>
        public int Count { get; set; }
    }
}
