/**********************************************************************
*******命名空间： SnAbp.File.OssSdk.Client
*******类 名 称： AliyunClient
*******类 说 明： 阿里云客户端方法封装及接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 15:07:47
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aliyun.OSS;
using Aliyun.OSS.Common;

namespace SnAbp.File.OssSdk.Client
{
    public class AliyunClient : OssAbstractClient
    {
        private readonly OssClient _client;
        private readonly string publicBucket;
        private readonly string securityBucket;
        private int expiresInt;

        public AliyunClient()
        {
        }

        public AliyunClient(OssOption option)
        {
            try
            {
                var config = new ClientConfiguration
                {
                    IsCname = true, // 支持自定义域名绑定
                    ConnectionTimeout = 2000 // 默认连接时间2s
                };
                _client = new OssClient(option.EndPoint, option.AccessKey, option.SecretKey, config);
                publicBucket = option.PublicBucket;
                securityBucket = option.SecurityBucket;
                expiresInt = option.Expires;
            }
            catch (ClientException ex)
            {
                _client = null;
            }
        }
        public override bool CheckStateAsync()
        {
            return CheckBucketAsync();
        }
        /// <summary>
        ///     检查存储桶是否存在，不存在则创建一个
        /// </summary>
        /// <returns></returns>
        private bool CheckBucketAsync()
        {
            try
            {
                if (_client == null)
                {
                    return false;
                }

                if (!ExistBucket(publicBucket).Result)
                {
                    // 设置访问权限为公开存取
                    var request = new CreateBucketRequest(publicBucket);
                    request.DataRedundancyType = DataRedundancyType.ZRS;
                    request.ACL = CannedAccessControlList.PublicReadWrite;
                    _client.CreateBucket(request);
                }

                if (!ExistBucket(securityBucket).Result)
                {
                    var request = new CreateBucketRequest(securityBucket);
                    request.DataRedundancyType = DataRedundancyType.ZRS;
                    request.ACL = CannedAccessControlList.Default;
                    _client.CreateBucket(request);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<bool> ExistBucket(string bucketName)
        {
            try
            {
                return await Task.FromResult(_client.DoesBucketExist(bucketName));
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Stream> GetObjectAsync(string objectName, bool isPublic = true)
        {
            try
            {
                if (!CheckBucketAsync())
                {
                    return null;
                }

                if (isPublic)
                {
                    return await Task.Run(() =>
                    {
                        var obj = _client.GetObject(publicBucket, objectName);
                        return obj.Content;
                    });
                }

                return await Task.Run(() =>
                {
                    var obj = _client.GetObject(securityBucket, objectName);
                    return obj.Content;
                });

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override async Task<string> GetObjectUrlAsync(string objectName, bool isPublic = true)
        {
            try
            {
                if (CheckBucketAsync())
                {
                    if (isPublic)
                    {
                        return await GetObjUrl(publicBucket, objectName, DateTime.Now.AddYears(100));
                    }
                }
                else
                {
                    return await GetObjUrl(securityBucket, objectName, DateTime.Now.AddDays(1));
                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public override async Task<string> GetPresignedUrl(string objectName, bool isPublic = true)
        {
            try
            {
                if (!CheckBucketAsync())
                {
                    return null;
                }

                if (isPublic)
                {
                    var con = new GeneratePresignedUriRequest(publicBucket, $"{GetBucketFolder()}{objectName}");
                    con.Method = SignHttpMethod.Put;
                    var url = _client.GeneratePresignedUri(con);
                    var urlStr = url.AbsoluteUri;
                    urlStr = urlStr.Replace("//", $"//{publicBucket}.");
                    return await Task.FromResult(urlStr);
                }
                else
                {
                    //私有文件7天过期时间
                    var url = _client.GeneratePresignedUri(securityBucket, $"{GetBucketFolder()}{objectName}", DateTime.Now.AddDays(7),
                        SignHttpMethod.Put);
                    var urlStr = url.AbsoluteUri;
                    urlStr = urlStr.Replace("//", $"//{securityBucket}.");
                    return await Task.FromResult(urlStr);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GetBucketFolder()
        {
            return $"{DateTime.Now.Year}/{DateTime.Now.Month.ToString().PadLeft(2, '0')}/";
        }
        public override async Task<string> PutObjectAsync(string objectName, Stream stream, bool isPublic = true)
        {
            try
            {
                if (CheckBucketAsync())
                {
                    var state = "success";
                    var evt = new AutoResetEvent(false);
                    if (isPublic)
                    {
                        _client.BeginPutObject(publicBucket, objectName, stream,
                            result => { _client.EndPutObject(result); }, state);
                        evt.WaitOne();
                        return await GetObjUrl(publicBucket, objectName, DateTime.Now.AddYears(100));
                    }

                    _client.BeginPutObject(securityBucket, objectName, stream, result => { _client.EndPutObject(result); },
                        state);
                    evt.WaitOne();
                    return await GetObjUrl(publicBucket, objectName, DateTime.Now.AddDays(1));
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        /// <summary>
        ///     获取指定文件的访问url
        /// </summary>
        /// <param name="bucketName">桶名称</param>
        /// <param name="key">文件唯一id</param>
        /// <param name="dt">过期时间</param>
        /// <returns></returns>
        private async Task<string> GetObjUrl(string bucketName, string key, DateTime dt)
        {
            var url = _client.GeneratePresignedUri(bucketName, key, dt, SignHttpMethod.Get);
            return await Task.FromResult(url.AbsoluteUri);
        }

        public override async Task RemoveObject(string objectName, bool isPublic = true)
        {
            try
            {
                if (CheckBucketAsync())
                {
                    if (isPublic) _client.DeleteObject(publicBucket, objectName);
                    else _client.DeleteObject(securityBucket, objectName);
                }
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public override Task GetObjectAsync(string objectName, Action<byte[]> action, bool isPublic = true)
        {
            throw new NotImplementedException();
        }
    }
}