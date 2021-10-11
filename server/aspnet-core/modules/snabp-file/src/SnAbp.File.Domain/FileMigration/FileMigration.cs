/**********************************************************************
*******命名空间： SnAbp.File.FileMigration
*******类 名 称： FileMigrationProvider
*******类 说 明： 文件迁移具体实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 8:45:33
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.File.Repositories;
using SnAbp.File.Services;
using SnAbp.Utils;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;

namespace SnAbp.File.FileMigration
{
    public class FileMigration : DomainService, IFileMigration
    {
        static FileMigrationConfig fileMigrationConfig;
        readonly ICustomRepository _customRepository;
        readonly FileMigrationCache FileMigrationCache;
        readonly IUnitOfWorkManager UnitOfWork;

        public FileMigration(FileMigrationCache fileMigrationCache, ICustomRepository customRepository,
            IUnitOfWorkManager unitOfWork)
        {
            FileMigrationCache = fileMigrationCache;
            _customRepository = customRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task CreateCache(FileMigrationState state)
        {
            await FileMigrationCache.Create(state);
        }

        public async Task<bool> CancelMigration()
        {
            var data = await FileMigrationCache.GetCache();
            if (data != null && !data.Cancel)
            {
                await FileMigrationCache.Update(1, true); // 关闭迁移
            }

            return true;
        }

        public async Task<FileMigrationState> DataContrast(FileMigrationConfig config)
        {
            fileMigrationConfig = config;
            var state = new FileMigrationState();
            // 先获取有没有正在进行的迁移过程，如果有则提示
            var cache = await FileMigrationCache.GetCache();

            if (cache != null && cache.IsStart)
            {
                return cache;
            }

            if (config == null)
            {
                return state;
            }

            var data1 = config.SourceFileVersions;
            var data2 = config.TargetFileVersion;

            if (data2 != null)
            {
                var Intersect = data1.Intersect(data2);
                state.Count = Intersect.Any() ? Intersect.Count() : data1.Count;
            }
            else
            {
                state.Count = data1?.Count ?? 0;
            }

            return state;
        }

        public async Task<FileMigrationState> ReadProcess()
        {
            var state = await FileMigrationCache.GetCache();
            return state ?? new FileMigrationState();
        }

        public Task StartMigration()
        {
            return Task.Run(async () =>
            {
                if (fileMigrationConfig == null)
                {
                    return;
                }

                // 获取源中得文件，再项目标服务中添加
                var sources = fileMigrationConfig.SourceFileVersions;
                foreach (var item in sources)
                {
                    // 获取当前的缓存状态，文件迁移是否被取消
                    var state = await FileMigrationCache.GetCache();
                    if (state != null && state.Cancel)
                    {
                        continue;
                    }

                    await fileMigrationConfig.SourceClient.GetObjectAsync(item.OssUrl, async data =>
                    {
                        // 获取到数据，进行保存
                        var fileStream = BytesToMemoryStream(data);
                        await fileMigrationConfig.TargetClient.PutObjectAsync(objectName: item.OssUrl.TrimStart('/'),
                            fileStream);
                        // 同时更新数据库中服务得地址及缓存
                        await _customRepository.UpdateFileVersion(item.Id, fileMigrationConfig.TargetGuid);
                        await FileMigrationCache.Update(sources.FindIndex(item) + 1); // 更新当前缓存
                    });
                }
            });
        }

        Stream BytesToMemoryStream(byte[] data)
        {
            Stream stream = new MemoryStream(data);
            return stream;
        }
    }
}