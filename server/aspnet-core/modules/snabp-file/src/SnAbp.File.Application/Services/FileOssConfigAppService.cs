/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： OssConfigAppService
*******类 说 明： 对象存储服务配置实现类
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 16:57:49
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SnAbp.File.Authorization;
using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.IServices;
using SnAbp.File.OssSdk;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.File.Services
{
    [Authorize]
    public class FileOssConfigAppService : FileAppService, IFileOssConfigAppService
    {
        private readonly IRepository<OssServer, Guid> _ossResp;
        private readonly IGuidGenerator _sequentialGuid;
        private readonly IOssRepository _ossClient;

        public FileOssConfigAppService(
            IGuidGenerator sequentialGuid,
            IRepository<OssServer, Guid> ossResp,
            IOssRepository ossClient
        )
        {
            _ossResp = ossResp;
            _sequentialGuid = sequentialGuid;
            _ossClient = ossClient;
        }

        /// <summary>
        ///     获取oss 配置列表
        /// </summary>
        /// <returns>oss 配置集合</returns>
        public Task<List<OssConfigDto>> GetList()
        {
            var list = new List<OssConfigDto>();
            var osslist = _ossResp.WithDetails(a=>a.FileVersions).ToList();
            osslist?.ForEach(a =>
            {
                var dto = new OssConfigDto();
                ObjectMapper.Map(a, dto);
                if (a.FileVersions != null)
                {
                    dto.Count = a.FileVersions.Count; 
                }
                //dto.State =_ossClient.GetServerState(a).Result;
                list.Add(dto);
            });
            return Task.FromResult(list);
        }

        /// <summary>
        /// 获取oss 对象存储服务的配置状态信息，此接口比较耗时，需要最后调用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<List<OssConfigStateDto>> GetOssState(Guid? id)
        {
            var list = new List<OssConfigStateDto>();
            var osslist = _ossResp.WithDetails(a => a.FileVersions).ToList();
            osslist?.ForEach(a =>
            {
                var dto = new OssConfigStateDto();
                dto.Id = a.Id;
                dto.State= _ossClient.GetServerState(a).Result;
                list.Add(dto);
            });
            return Task.FromResult(list);
        }

        /// <summary>
        ///     新增一个OSS 服务对象
        /// </summary>
        /// <param name="input">输入的对象</param>
        /// <returns>返回oss 对象</returns>
        [Authorize(FilePermissions.OssConfig.Create)]
        public async Task<OssConfigDto> Create(OssConfigInputDto input)
        {
            // 判断是否有重名
            var config = _ossResp.WhereIf(true, a => a.Name == input.Name);
            if (config.Any())
            {
                throw new UserFriendlyException($"已存在名称为：{input.Name}的配置了");
            }

            var model = new OssServer(_sequentialGuid.Create())
            {
                Name = input.Name,
                AccessKey = input.AccessKey,
                AccessSecret = input.AccessSecret,
                EndPoint = input.EndPoint,
                Type = input.Type
            };
            // 判断有几个服务
            if (!_ossResp.Any())
            {
                model.Enable = true;
            }
            var data = await _ossResp.InsertAsync(model);
            return ObjectMapper.Map<OssServer, OssConfigDto>(data);
        }

        /// <summary>
        ///     启动指定的服务
        /// </summary>
        /// <param name="id">oss id</param>
        /// <returns></returns>
        public async Task<bool> Enable(Guid id)
        {
            var list = await _ossResp.GetListAsync();
            if (list.Any())
            {
                // 设定需要启用的服务状态为true,其他的状态为false
                list?.ForEach(a =>
                {
                    a.Enable = a.Id == id;
                    _ossResp.UpdateAsync(a);
                });
                return true;
            }

            return false;
        }

        /// <summary>
        ///     清空指定oss 服务存储桶的所有数据
        /// </summary>
        /// <param name="id">oss 服务id</param>
        /// <returns></returns>
        [Authorize(FilePermissions.OssConfig.Delete)]
        public async Task<bool> Clear(Guid id)
        {
            // TODO 清空存储服务中的所有数据，需要调用接口,桶是删除指定的这一条记录
            await _ossResp.DeleteAsync(id);
            return await _ossClient.ClearBucket();
        }

        /// <summary>
        /// 服务状态测试s
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OssConfigDto> Check(Guid id)
        {
            var model = await _ossResp.GetAsync(id);
            var dto = ObjectMapper.Map<OssServer, OssConfigDto>(model);
            dto.State = await _ossClient.GetServerState(model);
            return dto;
        }

        /// <summary>
        ///  编辑oss 配置信息
        /// </summary>
        /// <param name="input">编辑的内容</param>
        /// <returns></returns>
        [Authorize(FilePermissions.OssConfig.Update)]
        public async Task<bool> Update(OssConfigUpdateDto input)
        {
            var model = await _ossResp.FindAsync(input.Id);
            if (model == null)
            {
                return false;
            }

            model.Type = input.Type;
            model.EndPoint = input.EndPoint;
            model.AccessSecret = input.AccessSecret;
            model.AccessKey = input.AccessKey;
            await _ossResp.UpdateAsync(model);
            return true;
        }
    }
}