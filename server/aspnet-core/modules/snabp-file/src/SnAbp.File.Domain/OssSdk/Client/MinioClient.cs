/**********************************************************************
*******命名空间： SnAbp.File.OssSdk.Client
*******类 名 称： MinioClient
*******类 说 明： Minio 客户端组件封装及接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 15:05:01
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Minio.Exceptions;
using Volo.Abp;
using static System.Environment;

namespace SnAbp.File.OssSdk.Client
{
    public class MinioClient : OssAbstractClient
    {
        private readonly string publicBucket;
        private readonly string securityBucket;
        private readonly Minio.MinioClient _client;
        private int expiresInt;

        public MinioClient()
        {
        }

        public MinioClient(OssOption option)
        {
            try
            {
                var enableHTTPS = false;
                if (string.IsNullOrEmpty(option.EndPoint) && string.IsNullOrEmpty(option.AccessKey) &&
                    string.IsNullOrEmpty(option.SecretKey))
                {
                    if (GetEnvironmentVariable("SERVER_ENDPOINT") != null)
                    {
                        if (GetEnvironmentVariable("ENABLE_HTTPS") != null)
                        {
                            // ReSharper disable once PossibleNullReferenceException
                            enableHTTPS = GetEnvironmentVariable($"ENABLE_HTTPS").Equals("1");
                        }
                    }
                }

                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;

                _client = enableHTTPS
                    ? new Minio.MinioClient(option.EndPoint, option.AccessKey, option.SecretKey).WithSSL()
                    : new Minio.MinioClient(option.EndPoint, option.AccessKey, option.SecretKey);
                //.WithSSL();
                publicBucket = option.PublicBucket;
                securityBucket = option.SecurityBucket;
                expiresInt = option.Expires;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("对象存储服务配置异常");
            }
        }

        public override bool CheckStateAsync()
        {
            return CheckBucketAsync().Result;
        }

        /// <summary>
        ///     判断指定桶是否存在,如果不存在，则重新创建一个桶
        /// </summary>
        /// <param name="bucketName">桶名</param>
        /// <returns></returns>
        private async Task<bool> CheckBucketAsync()
        {
            try
            {
                if (_client == null)
                {
                    return false;
                }

                if (!await ExistBucket(publicBucket))
                {
                    await _client.MakeBucketAsync(publicBucket);
                    // 同时创建一个可读写的策略
                    await CreateBucketPolicy(publicBucket);
                }

                if (!await ExistBucket(securityBucket))
                {
                    await _client.MakeBucketAsync(securityBucket);
                    // 同时创建一个可读写的策略
                    await CreateBucketPolicy(publicBucket);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task CreateBucketPolicy(string bucketName)
        {
            var policyJson = $@"{{ ""Version"":""2012-10-17"",""Statement"":[{{ ""Effect"":""Allow"",""Principal"":{{ ""AWS"":[""*""]}},""Action"":[""s3:ListBucketMultipartUploads"",""s3:GetBucketLocation"",""s3:ListBucket""],""Resource"":[""arn:aws:s3:::{bucketName}""]}},{{ ""Effect"":""Allow"",""Principal"":{{ ""AWS"":[""*""]}},""Action"":[""s3:AbortMultipartUpload"",""s3:DeleteObject"",""s3:GetObject"",""s3:ListMultipartUploadParts"",""s3:PutObject""],""Resource"":[""arn:aws:s3:::{bucketName}/*""]}}]}}";
            await _client.SetPolicyAsync(bucketName, policyJson);
        }

        private async Task<bool> ExistBucket(string bucketName)
        {
            return await _client.BucketExistsAsync(bucketName);
        }

        public override async Task<string> GetPresignedUrl(string objectName, bool isPublic = true)
        {
            try
            {
                await CheckBucketAsync();
                if (isPublic)
                {
                    return await _client.PresignedPutObjectAsync(publicBucket, $"{GetBucketFolder()}{objectName}", expiresInt);
                }

                return await _client.PresignedPutObjectAsync(securityBucket, $"{GetBucketFolder()}{objectName}", expiresInt);
            }
            catch (MinioException ex)
            {
                return null;
            }
        }
        private string GetBucketFolder()
        {
            return $"{DateTime.Now.Year}/{DateTime.Now.Month.ToString().PadLeft(2, '0')}/";
        }
        public override async Task<string> GetObjectUrlAsync(string objectName, bool isPublic)
        {
            try
            {
                expiresInt = 60 * 60;
                await CheckBucketAsync();
                if (isPublic)
                {
                    return await _client.PresignedGetObjectAsync(publicBucket, objectName, expiresInt * int.MaxValue);
                }

                return await _client.PresignedGetObjectAsync(securityBucket, objectName, expiresInt * 12);
            }
            catch (MinioException ex)
            {
                return null;
            }
        }

        public override async Task GetObjectAsync(string objectName, Action<byte[]> action, bool isPublic = true)
        {
            try
            {
                await CheckBucketAsync();
                if (isPublic)
                {
                    var data = await _client.StatObjectAsync(publicBucket, objectName);
                    await _client.GetObjectAsync(publicBucket, objectName, stream =>
                    {
                        var buffer = StreamToBytes(stream, data.Size);
                        action(buffer);
                    });
                }
                else
                {
                    // await _client.GetObjectAsync(securityBucket, objectName, action);
                }
            }
            catch (MinioException ex)
            {
                action(null);
            }
        }

        private byte[] StreamToBytes(Stream stream, long size)
        {
            byte[] bytes = new byte[size];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public override async Task<string> PutObjectAsync(string objectName, Stream stream, bool isPublic = true)
        {
            try
            {
                await CheckBucketAsync();
                if (isPublic)
                {
                    return await PutObjectAsync(publicBucket, objectName, stream, expiresInt * int.MaxValue);
                }

                return await PutObjectAsync(publicBucket, objectName, stream, expiresInt * 12);
            }
            catch (MinioException ex)
            {
                return null;
            }
        }

        /// <summary>
        ///     以文件流的形式保存对象，并返回文件的访问url
        /// </summary>
        /// <param name="bucketName">桶名</param>
        /// <param name="key">文件唯一key</param>
        /// <param name="stream">文件流</param>
        /// <param name="expiresInt">过期时间</param>
        /// <returns></returns>
        private async Task<string> PutObjectAsync(string bucketName, string key, Stream stream, int expiresInt)
        {
            try
            {
                await _client.PutObjectAsync(bucketName, key, stream, stream.Length, "application/octet-stream");
                return await GetObjectUrlAsync(bucketName, key, expiresInt);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///     获取指定文件的get访问路径
        /// </summary>
        /// <param name="bucketName">桶名称</param>
        /// <param name="key">文件的唯一key</param>
        /// <returns></returns>
        private async Task<string> GetObjectUrlAsync(string bucketName, string key, int expiresInt)
        {
            try
            {
                return await _client.PresignedGetObjectAsync(bucketName, key, expiresInt);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除资源信息
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public override async Task RemoveObject(string objectName, bool isPublic = true)
        {
            try
            {
                if (await CheckBucketAsync())
                {
                    if (isPublic) await _client.RemoveObjectAsync(publicBucket, objectName);
                    else await _client.RemoveObjectAsync(securityBucket, objectName);
                }
            }
            catch (Exception e)
            {


            }
        }
    }
}