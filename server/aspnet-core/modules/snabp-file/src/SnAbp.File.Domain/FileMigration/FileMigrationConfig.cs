/**********************************************************************
*******命名空间： SnAbp.File.FileMigration
*******类 名 称： FileMigrationConfig
*******类 说 明： 文件迁移配置
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 8:41:37
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.File.Entities;
using SnAbp.File.OssSdk.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnAbp.File
{
    public class FileMigrationConfig
    {
        /// <summary>
        /// 数据源对象客户端
        /// </summary>
        public OssAbstractClient SourceClient { get; set; }

        /// <summary>
        /// 目标对象客户端
        /// </summary>
        public OssAbstractClient TargetClient { get; set; }

        /// <summary>
        /// 源配置
        /// </summary>
        public List<FileVersion> SourceFileVersions { get; set; }

        /// <summary>
        /// 目标服务配置
        /// </summary>
        public List<FileVersion> TargetFileVersion { get; set; }

        public Guid TargetGuid { get; set; }
    }
}
