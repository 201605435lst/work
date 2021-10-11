/**********************************************************************
*******命名空间： SnAbp.Common.Services
*******接口名称： IFileImportHandler
*******接口说明： 文件导入帮助类，用到公共组件导入时，需要在对应的服务中注入此依赖
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 10:23:52
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SnAbp.Common
{
    public interface IFileImportHandler:ITransientDependency
    {
        /// <summary>
        /// 导入开始
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <returns></returns>
        Task Start(string key, decimal dataCount = 0);


        /// <summary>
        /// 更改总数 会改变原有进度 勿在实际进行中调用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Task ChangeTotalCount(string key, decimal count);

        /// <summary>
        /// 文件导入更新，正常在执行完一次数据库操作后执行一次此方法
        /// </summary>
        /// <param name="key">唯一标识</param>
        /// <returns></returns>

        Task UpdateState(string key, decimal index);

        /// <summary>
        /// 执行完文件导入操作后执行此方法
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task Complete(string key);
        
        /// <summary>
        /// 取消导入
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task Cancel(string key);

        /// <summary>
        /// 保存文件字节流
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        Task SaveExceptionFile(Guid useId,string key,byte[] fileBytes);

        Task RemoveCache(string key);

        /// <summary>
        /// 保存导出的文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        Task SaveExportStream(byte[] bytes, string key);

    }
}
