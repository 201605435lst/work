/**********************************************************************
*******命名空间： SnAbp.File.FileMigration
*******类 名 称： FileMigrationState
*******类 说 明： 文件迁移状态
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 8:43:00
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File
{
    public class FileMigrationState
    {
        /// <summary>
        /// 是否开始迁移
        /// </summary>
        public bool IsStart { get; set; }

        /// <summary>
        /// 是否迁移成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 是否取消迁移
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// 迁移进度信息
        /// </summary>
        public int Progress { get; set; }

        /// <summary>
        /// 迁移文件的总条数
        /// </summary>
        public int Count { get; set; }

        public  DateTime LastUpdateTime { get; set; }

    }
}
