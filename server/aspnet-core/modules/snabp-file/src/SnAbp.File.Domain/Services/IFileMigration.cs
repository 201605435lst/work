/**********************************************************************
*******命名空间： SnAbp.File.FileMigration
*******接口名称： IFileMigration
*******接口说明： 文件迁移接口，用于直接对接服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 8:44:13
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace SnAbp.File.Services
{
    public interface IFileMigration: IDomainService
    {
        /// <summary>
        /// 数据比较
        /// </summary>
        /// <returns></returns>
        Task<FileMigrationState> DataContrast(FileMigrationConfig config);

        /// <summary>
        /// 开始迁移
        /// </summary>
        /// <returns></returns>
        Task StartMigration();

        /// <summary>
        /// 取消迁移
        /// </summary>
        /// <returns></returns>
        Task<bool> CancelMigration();

        /// <summary>
        /// 读取进度
        /// </summary>
        /// <returns></returns>
        Task<FileMigrationState> ReadProcess();

        /// <summary>
        /// 添加缓存信息
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task CreateCache(FileMigrationState state);
    }
}
