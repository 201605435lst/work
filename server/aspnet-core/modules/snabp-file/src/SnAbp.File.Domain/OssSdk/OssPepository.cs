/**********************************************************************
*******命名空间： SnAbp.File.OssSdk
*******类 名 称： OssPepository
*******类 说 明： 
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 18:01:05
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SnAbp.File.Entities;
using SnAbp.File.OssSdk.Client;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.File.OssSdk
{
    public class OssRepository : IOssRepository
    {
        private readonly OssAbstractClient _client;
        private readonly IConfiguration _configuration;
        private readonly IRepository<OssServer, Guid> _ossResp;

        public OssRepository(
            IRepository<OssServer, Guid> ossResp,
            IConfiguration configuration
        )
        {
            _ossResp = ossResp;
            _configuration = configuration;
            _client = OssAbstractClient.CreateClient(GetOssOptionAsync());
        }

        public OssServer OssServer => GetOssServer();


        public Task<bool> ClearBucket() => Task.FromResult(true);

        public async Task Delete(string key)
        {
            await _client.RemoveObject(key);
        }
        /// <summary>
        /// 根据文件地址获取文件流
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileStream"></param>
        public void GetFileByUrl(string url, Action<Stream> fileStream)
        {
            _client.GetObjectAsync(url, buffers => fileStream.Invoke(new MemoryStream(buffers)));
        }

        public async Task<string> GetFileUrl(string key)
        {
            if (_client != null)
            {
                return await _client.GetObjectUrlAsync(key);
            }
            else
            {
                return "";
            }

        }

        public async Task<string> GetPresignUrl(string key)
        {
            if (_client != null)
            {
                return await _client.GetPresignedUrl(key);
            }
            else
            {
                return "";
            }

        }

        public async Task<string> GetServerState(OssServer model)
        {
            var state = string.Empty;
            if (_client == null)
                return "";
            await Task.Run(() => { state = _client.GetServerState(GetOssOptionAsync(model)); });
            return state;
        }

        public async Task PutObject(Stream stream, string fileName)
        {
            await _client.PutObjectAsync(fileName, stream);
        }

        private OssOption GetOssOptionAsync()
        {
            try
            {
                OssOption option = null;
                // 新增处理，当数据库中就条服务数据时，就让其启动并
                var ossModel = _ossResp.FirstOrDefault(a => a.Enable);
                return ossModel != null ? GetOssOptionAsync(ossModel) : option;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("请配置服务");
            }
        }

        private OssOption GetOssOptionAsync(OssServer ossModel)
        {
            if (ossModel == null)
            {
                return null;
            }

            var option = new OssOption
            {
                AccessKey = ossModel.AccessKey,
                SecretKey = ossModel.AccessSecret,
                EndPoint = ossModel.EndPoint,
                Expires = 60 * 60,
                PublicBucket = _configuration["OssConfig:PublicBucket"],
                SecurityBucket = _configuration["OssConfig:SecurityBucket"],
                Type = ossModel.Type
            };

            return option;
        }
        private OssServer GetOssServer()
        {
            return _ossResp.SingleOrDefault(a => a.Enable); ;
        }

    }
}