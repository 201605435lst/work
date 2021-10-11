/**********************************************************************
*******命名空间： SnAbp.Common.Services
*******类 名 称： FileImportHandler
*******类 说 明： 文件导入查询逻辑实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 10:25:56
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SnAbp.Common.Dtos;
using Volo.Abp.Caching;
using Volo.Abp.Users;

namespace SnAbp.Common
{
    public class FileImportHandler : IFileImportHandler
    {
        protected IDistributedCache<FileImportDto> _importCache; // 文件导入缓存
        protected IDistributedCache<FileCacheModal> _streamCache; // 文件导入缓存


        public FileImportHandler(IDistributedCache<FileImportDto> importCache,
            IDistributedCache<FileCacheModal> streamCache)
        {
            _streamCache = streamCache;
            _importCache = importCache;
        }

        decimal _dataCount;
        decimal _currentIndex;
        public async Task Start(string key, decimal dataCount = 1)
        {
            this._dataCount = dataCount;
            var cache = await _importCache.GetAsync(key);
            if (cache == null)
            {
                var dto = new FileImportDto
                {
                    CacheKey = key,
                    Progress = 0
                };
                await _importCache.SetAsync(key, dto);
            }
        }

        /// <summary>
        /// 更改总数 会改变原有进度 勿在实际进行中调用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task ChangeTotalCount(string key, decimal count)
        {
            var cache = await _importCache.GetAsync(key);
            if (cache != null)
            {
                this._dataCount = count;
                cache.Progress = this.CalculationProgress();
                cache.UpdateTime = DateTime.Now;
                await _importCache.SetAsync(key, cache);
            }
        }

        public void SetDateParameters(decimal dataCount)
        {
            this._dataCount = dataCount;
        }

        /// <summary>
        ///  获取导入状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public async Task<FileImportDto> GetImportState(string key)
        {
            var cache = await _importCache.GetAsync(key);
            if (cache == null)
            {
                return null;
            }

            if (cache.Progress != 1)
            {
                return cache;
            }

            if (cache.Success)
            {
                var caches = cache;
                await _importCache.RemoveAsync(key);
                return caches;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateState(string key, decimal index)
        {
            this._currentIndex = index;
            var cache = await _importCache.GetAsync(key);
            if (cache == null) return;
            cache.Progress = this.CalculationProgress();
            if (cache.Progress == 1)
                cache.Success = false;
            cache.UpdateTime = DateTime.Now;
            await _importCache.SetAsync(key, cache);
        }

        public async Task Complete(string key)
        {
            var cache = await _importCache.GetAsync(key);
            if (cache == null) return;
            cache.Progress = 1;
            cache.Success = true;
            await _importCache.SetAsync(key, cache);
        }

        public async Task Cancel(string key)
        {
            var cache = await _importCache.GetAsync(key);
            if (cache == null) return;
            await _importCache.RemoveAsync(key);
            await _streamCache.RemoveAsync(key);
        }

        /// <summary>
        /// 计算进度数据
        /// </summary>
        /// <returns></returns>
        private decimal CalculationProgress()
        {
            try
            {
                var res = this._currentIndex / this._dataCount;
                int temp = (int)(res * 100);
                res = (decimal)temp / 100;
                //将四舍五入的计算方式改为截取两位小数
                //Math.Round(d: this._currentIndex / this._dataCount, 2);
                return res;
            }
            catch (Exception)
            {

                return 1;
            }

        }

        /// <summary>
        /// 保存异常文件信息，一般是导入的文件中加入异常的行的数据，然后导入给用户。
        /// </summary>
        /// <param name="useId">用户id</param>
        /// <param name="key">文件导入的唯一标识,文件缓存的key 格式为：file-key</param>
        /// <param name="fileName">需要下载的文件名称</param>
        /// <param name="fileBytes"></param>
        /// <returns></returns>
        public async Task SaveExceptionFile(Guid useId, string key, byte[] fileBytes)
        {
            var importKey = $"{useId}{key}";
            var cache = await _streamCache.GetAsync(importKey);
            if (cache != null)
            {
                cache.FileBytes = fileBytes;
            }
            else
            {
                var dto = new FileCacheModal() { FileBytes = fileBytes };
                await _streamCache.SetAsync(importKey, dto);
            }
        }

        /// <summary>
        /// 获取需要下载的文档，获取完文档后就执行缓存的删除操作
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public FileExportDto GetDownloadFile(string key)
        {
            var cache = _streamCache.Get(key);
            if (cache == null)
            {
                return null;
            }
            var dto = new FileExportDto()
            {
                FileName = cache.FileName,
                Stream = new MemoryStream(cache.FileBytes)
            };
            return dto;

        }

        /// <summary>
        /// 移除指定的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveCache(string key) => await _streamCache.RemoveAsync(key);

        /// <summary>
        /// 保存文件流数据
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public async Task SaveExportStream(byte[] bytes, string fileKey)
        {
            var dto = new FileCacheModal()
            {
                FileBytes = bytes,
                FileName = fileKey
            };
            await _streamCache.SetAsync(fileKey, dto);
        }
    }
}
