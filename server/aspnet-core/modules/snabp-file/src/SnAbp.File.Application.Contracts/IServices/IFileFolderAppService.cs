/**********************************************************************
*******命名空间： SnAbp.File.IServices
*******接口名称： IFileFolderAppService
*******接口说明： 文件夹管理服务接口，包括基础的维护功能，业务享关功能在其他接口定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 17:39:35
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnAbp.File.Dtos;
using SnAbp.File.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.File.IServices
{
    public interface IFileFolderAppService : IApplicationService
    {
        Task<FileFolderDto> Get(Guid id);

        Task<List<FileDownloadDto>> GetDownloadFile(Guid id);
        Task<bool> Create(FileFolderInputDto input);
        Task<bool> Update(FileFolderUpdateDto input);
        Task<bool> Delete(Guid id);
    }
}