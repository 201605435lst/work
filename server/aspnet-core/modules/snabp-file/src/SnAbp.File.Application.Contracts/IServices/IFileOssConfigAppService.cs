/**********************************************************************
*******命名空间： SnAbp.File.IServices
*******接口名称： IOssServerConfigAppService
*******接口说明： 对象存储服务配置接口
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/9 14:10:58
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.File.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.File.IServices
{
    public interface IFileOssConfigAppService : IApplicationService
    {
        Task<OssConfigDto> Create(OssConfigInputDto input);
        Task<bool> Update(OssConfigUpdateDto input);
        Task<List<OssConfigDto>> GetList();
        Task<bool> Enable(Guid id);
        Task<bool> Clear(Guid id);
        Task<OssConfigDto> Check(Guid id);
        Task<List<OssConfigStateDto>> GetOssState(Guid? id);
    }
}