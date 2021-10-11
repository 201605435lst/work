/**********************************************************************
*******命名空间： SnAbp.File.OssSdk
*******接口名称： IOssPepository
*******接口说明： Oss 对象存储公共接口，用来定义对象存储使用的公用组件
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 15:02:39
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.IO;
using System.Threading.Tasks;
using SnAbp.File.Entities;
using Volo.Abp.DependencyInjection;

namespace SnAbp.File.OssSdk
{
    public interface IOssRepository : ITransientDependency
    {
        /// <summary>
        ///     OSS 服务对象
        /// </summary>
        OssServer OssServer { get; }


        Task<bool> ClearBucket();

        /// <summary>
        ///     根据key 获取oss 服务中的地址，key为具体的文件名加后缀
        /// </summary>
        /// <param name="key">文件名，例如：xxxxx.png </param>
        /// <returns></returns>
        Task<string> GetFileUrl(string key);

        /// <summary>
        ///     根据文件key名称获取文件上传签名地址
        /// </summary>
        /// <param name="key">文件名称</param>
        /// <returns>返回用于上传文件的签名地址</returns>
        Task<string> GetPresignUrl(string key);

        /// <summary>
        /// 根据类型判断当前服务的运行状态
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<string> GetServerState(OssServer model);

        /// <summary>
        /// 删除指定的资源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task Delete(string key);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task PutObject(Stream stream, string fileName);

        /// <summary>
        /// 根据文件url获取文件流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileStream"></param>
        void GetFileByUrl(string url, Action<Stream> fileStream);
    }
}