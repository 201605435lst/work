/**********************************************************************
*******命名空间： SnAbp.File.OssSdk.Client
*******类 名 称： AmanzonS3Client
*******类 说 明： 亚马逊s3对象存储客户端封装及接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/10 15:09:47
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace SnAbp.File.OssSdk.Client
{
   public class AmanzonS3Client : OssAbstractClient
    {
        private readonly AmazonS3Client _client;
        private readonly string publicBucket;
        private readonly string securityBucket;
        private int expiresInt;

        public AmanzonS3Client()
        {
        }

        public AmanzonS3Client(OssOption option)
        {
            try
            {
                publicBucket = option.PublicBucket;
                securityBucket = option.SecurityBucket;
                var config = new AmazonS3Config
                {
                    UseHttp = true
                };
                _client = new AmazonS3Client(option.AccessKey, option.SecretKey, config);
            }
            catch (Exception)
            {
                _client = null;
            }
        }
        public override bool CheckStateAsync()
        {
            //CheckBucketAsync();
            return false;
        }

        public async Task<Stream> GetObjectAsync(string objectName, bool isPublic = true)
        {
            try
            {
                if (isPublic)
                {
                    var res = await _client.GetObjectAsync(publicBucket, objectName);
                    return res.ResponseStream;
                }
                else
                {
                    var res = await _client.GetObjectAsync(securityBucket, objectName);
                    return res.ResponseStream;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override Task GetObjectAsync(string objectName, Action<byte[]> action, bool isPublic = true)
        {
            throw new NotImplementedException();
        }

        public override async Task<string> GetObjectUrlAsync(string objectName, bool isPublic = true)
        {
            try
            {
                if (isPublic)
                {
                    return await GetObjectUrlAsync(publicBucket, objectName);
                }

                return await GetObjectUrlAsync(securityBucket, objectName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public override async Task<string> GetPresignedUrl(string objectName, bool isPublic = true)
        {
            try
            {
                var request = new GetPreSignedUrlRequest();
                request.Key = objectName;
                request.Protocol = Protocol.HTTP;
                request.Verb = HttpVerb.PUT;

                if (isPublic)
                {
                    request.BucketName = publicBucket;
                    request.Expires = DateTime.Now.AddYears(100);
                    var preUrl = _client.GetPreSignedURL(request);
                    return await Task.FromResult(preUrl);
                }
                else
                {
                    request.BucketName = securityBucket;
                    request.Expires = DateTime.Now.AddHours(24);
                    var preUrl = _client.GetPreSignedURL(request);
                    return await Task.FromResult(preUrl);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override async Task<string> PutObjectAsync(string objectName, Stream stream, bool isPublic = true)
        {
            try
            {
                var request = new PutObjectRequest();
                request.Key = objectName;
                request.InputStream = stream;
                if (isPublic)
                {
                    request.BucketName = publicBucket;
                    await _client.PutObjectAsync(request);
                }
                else
                {
                    request.BucketName = securityBucket;
                    await _client.PutObjectAsync(request);
                }

                return await GetObjectUrlAsync(request.BucketName, request.Key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>

        public override async Task RemoveObject(string objectName, bool isPublic = true)
        {
            try
            {
                if (isPublic)
                {
                   await  _client.DeleteObjectAsync(publicBucket, objectName);
                }
                else
                {
                   await _client.DeleteObjectAsync(securityBucket, objectName);
                }
            }
            catch (Exception e)
            {
                
            }
        }

        /// <summary>
        ///     获取指定对象的访问url
        /// </summary>
        /// <param name="bucketName">桶名</param>
        /// <param name="key">文件的唯一key</param>
        /// <returns></returns>
        private async Task<string> GetObjectUrlAsync(string bucketName, string key)
        {
            try
            {
                var response = await _client.GetObjectAsync(bucketName, key);
                return response.WebsiteRedirectLocation;
            }
            catch (Exception EX)
            {
                return null;
            }
        }

        //private async Task CheckBucketAsync()
        //{
        //    if (_client == null)
        //        return;
        //    try
        //    {
        //        if (await ExistBucket(publicBucket)) { }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private async Task<bool> ExistBucket(string bucketName)
        //{
        //    try
        //    {
        //        var response = await _client.GetBucketLocationAsync(publicBucket);
        //        return string.IsNullOrEmpty(response.Location.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
    }
}