/**********************************************************************
*******命名空间： SnAbp.Common.Services
*******类 名 称： CommonFileImportAppService
*******类 说 明： 文件导入服务接口
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/2 10:59:33
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Common.Dtos;
using SnAbp.Common.IServices;
using System.Net;
using Volo.Abp;
using System;
using SnAbp.Common.Dtos.Task;
using Volo.Abp.Caching;
using System.Threading;

namespace SnAbp.Common.Services
{
    public class CommonBackgroundTaskAppService : CommonAppService, ICommonBackgroundTaskAppService
    {
        private IDistributedCache<BackgroundTaskDto> _taskCache;

        public CommonBackgroundTaskAppService(IDistributedCache<BackgroundTaskDto> taskCache)
        {
            _taskCache = taskCache;
        }

        public async Task<BackgroundTaskDto> Get(string key)
        {
            return await _taskCache.GetAsync(key);
        }

        public async Task<BackgroundTaskDto> Create(BackgroundTaskDto input)
        {
            await _taskCache.SetAsync(input.Key, input);
            return await _taskCache.GetAsync(input.Key); ;
        }

        public async Task<BackgroundTaskDto> Update(BackgroundTaskDto input)
        {
            var task = await _taskCache.GetAsync(input.Key);
            if (task != null)
            {
                task.Index = input.Index;
                task.IsDone = input.IsDone;
                task.Message = input.Message;
                task.HasError = input.HasError;
                task.Count = input.Count;

                await _taskCache.SetAsync(input.Key, task);
                if (task.Index == task.Count)
                {
                    await Done(input.Key);
                }
                return await _taskCache.GetAsync(input.Key);
            }
            return null;

        }

        public async Task<bool> Done(string key)
        {
            var task = await _taskCache.GetAsync(key);
            if (task != null)
            {
                task.IsDone = true;
                await _taskCache.SetAsync(key, task);
                Thread.Sleep(10000);
                await Cancel(key);
                return true;
            }

            return false;
        }

        public async Task<bool> Cancel(string key)
        {
            await _taskCache.RemoveAsync(key);
            return true;
        }
    }
}
