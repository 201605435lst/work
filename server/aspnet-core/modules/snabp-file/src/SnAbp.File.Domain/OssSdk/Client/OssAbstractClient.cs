/**********************************************************************
*******命名空间： SnAbp.File.OssSdk.Client
*******类 名 称： OssClient
*******类 说 明： 抽象Oss 存储服务客户端
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 18:03:48
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.IO;
using System.Threading.Tasks;

namespace SnAbp.File.OssSdk.Client
{
    public abstract class OssAbstractClient
    {
        public static OssAbstractClient CreateClient(OssOption option)
        {
            OssAbstractClient client = null;
            if (option == null)
            {
                return null;
            }

            if (option.Type == OssServerType.Aliyun)
            {
                client = new AliyunClient(option);
            }
            else if (option.Type == OssServerType.MinIO)
            {
                client = new MinioClient(option);
            }
            else if (option.Type == OssServerType.AmazonS3)
            {
                client = new AmanzonS3Client(option);
            }

            return client;
        }

        public string GetServerState(OssOption option)
        {
            try
            {
                var client = CreateClient(option);
                if (client != null)
                {
                    return client.CheckStateAsync() ? "正常" : "服务异常,请检查配置";
                }
                return "服务异常，请检查配置";
            }
            catch (Exception e)
            {
                return @"服务异常，请检查配置";
            }
        }

        /// <summary>
        ///     获取上传资源的签名地址，设置过期时间为24小时
        /// </summary>
        /// <param name="objectName">需要上传的文件名称</param>
        /// <returns></returns>
        public abstract Task<string> GetPresignedUrl(string objectName, bool isPublic = true);

        /// <summary>
        ///     获取存储桶中指定文件的访问地址地址,公共桶中的资源为永久不过期，私有桶中资源默认过期时间为7天
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public abstract Task<string> GetObjectUrlAsync(string objectName, bool isPublic = true);

        /// <summary>
        ///     获取指定对象的流数据
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public abstract Task GetObjectAsync(string objectName, Action<byte[]> action, bool isPublic = true);

        /// <summary>
        ///     通过流形式保存对象,并返回保存后的文件访问地址
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public abstract Task<string> PutObjectAsync(string objectName, Stream stream, bool isPublic = true);

        public abstract bool CheckStateAsync();

        public abstract Task RemoveObject(string objectName, bool isPublic = true);
    }
}