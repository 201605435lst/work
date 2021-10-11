/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： FileMigrationAppService
*******类 说 明： 文件迁移服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/9/30 11:31:53
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.OssSdk;
using SnAbp.File.OssSdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnAbp.File.IServices;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.File.Services
{
    public class FileMigrationAppService:FileAppService, IFileMigrationAppService
    {
        private readonly IFileMigration FileMigration;
        private readonly IConfiguration Configuration;
        private readonly IRepository<OssServer, Guid> OssRepository;

        public FileMigrationAppService(
            IFileMigration fileMigration,
            IConfiguration configuration,
            IRepository<OssServer, Guid> ossRepository)
        {
            FileMigration = fileMigration;
            OssRepository = ossRepository;
            Configuration = configuration;
        }

        /// <summary>
        /// 文件对比
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<FileMigrationDto> DataContrast(FileMigrationInputDto input)
        {
            var sourceOss = OssRepository
                .WithDetails(a=>a.FileVersions)
                .FirstOrDefault(a => a.Id == input.SrouceId);
            var targetOss = OssRepository
                .WithDetails(a=>a.FileVersions)
                .FirstOrDefault(a => a.Id == input.TargetId);
            var fileMigrationConfig = new FileMigrationConfig
            {
                SourceClient = OssAbstractClient.CreateClient(GetOssOptionAsync(sourceOss)),
                TargetClient = OssAbstractClient.CreateClient(GetOssOptionAsync(targetOss)),
                SourceFileVersions = sourceOss.FileVersions.Select(a=>new FileVersion(a.Id){OssUrl = a.OssUrl}).ToList(),
                TargetFileVersion = targetOss.FileVersions.Select(a => new FileVersion(a.Id) { OssUrl = a.OssUrl }).ToList(),
                TargetGuid = targetOss.Id
            };

            var result =await FileMigration.DataContrast(fileMigrationConfig);
            // 同时添加到缓存中
            await FileMigration.CreateCache(result);
            return ObjectMapper.Map<FileMigrationState, FileMigrationDto>(result);
        }

        /// <summary>
        /// 开始迁移
        /// </summary>
        /// <returns></returns>
        public async Task<FileMigrationDto> Start()
        {
           _=FileMigration.StartMigration(); // 无需等待，直接执行。
            return await GetProcess();
        }

        /// <summary>
        /// 获取进度
        /// </summary>
        /// <returns></returns>
        public async Task<FileMigrationDto> GetProcess()
        {
            var result = await FileMigration.ReadProcess();
            return ObjectMapper.Map<FileMigrationState, FileMigrationDto>(result);
        }

        /// <summary>
        /// 关闭迁移
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Cancel() => await FileMigration.CancelMigration();

        /// <summary>
        /// 获取服务配置
        /// </summary>
        /// <param name="ossModel"></param>
        /// <returns></returns>
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
                PublicBucket = Configuration["OssConfig:PublicBucket"],
                SecurityBucket = Configuration["OssConfig:SecurityBucket"],
                Type = ossModel.Type
            };

            return option;
        }


    }
}
