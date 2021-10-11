/**********************************************************************
*******命名空间： SnAbp.File.FileMigration
*******类 名 称： FileMigrationCache
*******类 说 明： 文件迁移缓存封装
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 9:09:48
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;

namespace SnAbp.File.FileMigration
{
    public class FileMigrationCache: DomainService
    {
        private const string CacheKey = "fileMigaration";
        private IDistributedCache<FileMigrationState> DistributedCache;

        public FileMigrationCache(IDistributedCache<FileMigrationState> distributedCache) => DistributedCache = distributedCache;

        /// <summary>
        /// 获取当前迁移缓存数据
        /// </summary>
        /// <returns></returns>
        public async Task<FileMigrationState> GetCache()
        {
            var state = await DistributedCache.GetAsync(CacheKey);
            if (state == null)
            {
                return null;
            }
            if (!state.Success)
            {
                return state;
            }

            if (!((DateTime.Now - state.LastUpdateTime).TotalSeconds > 2))
            {
                return state;
            }

            await Clear();
            return null;
        }

        /// <summary>
        /// 更新当前进度
        /// </summary>
        /// <param name="index">处理的索引值</param>
        /// <param name="cancel">当前进程是否处理有误</param>
        /// <returns></returns>
        public async Task<FileMigrationState> Update(int index, bool cancel = false)
        {
            try
            {
                var state = await GetCache();
                if (state == null)
                {
                    return null;
                }

                if (cancel)
                {
                    // 取消迁移
                    state.Cancel = true;
                    state.Progress = 1;
                    state.Success = true;
                }
                else
                {
                    state.Progress = (int) ((index / (float)(state.Count)) * 100);
                }

                if (index == state.Count)
                {
                    state.Progress = 100;
                    state.Success = true;
                }
                state.LastUpdateTime=DateTime.Now;
                await DistributedCache.SetAsync(CacheKey, state);
                return state;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建一个缓存
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task Create(FileMigrationState state)
        {
            await DistributedCache.SetAsync(CacheKey, state);
        }


        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Clear()
        {
            try
            {
               await DistributedCache.RemoveAsync(CacheKey);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
