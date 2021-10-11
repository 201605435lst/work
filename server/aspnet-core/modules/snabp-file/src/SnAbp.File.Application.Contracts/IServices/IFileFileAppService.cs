/**********************************************************************
*******命名空间： SnAbp.File.IServices
*******接口名称： IFileAppService
*******接口说明： 文件管理维护接口定义，包括文件新增，编辑和删除，权限分享等控制在文件管理接口中定义
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/12 9:15:37
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SnAbp.File.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.File.IServices
{
    public interface IFileFileAppService : IApplicationService
    {
        Task<FileDto> GetPresignUrl(FileInputDto input);
        Task<bool> Create(FileCreateDto input);
        Task<bool> Update(FileUpdateDto input);
        Task<FileSimpleDto> UploadForApp([FromForm] FileUploadDto input);
        Task<bool> Delete(Guid id);

        /***************文件版本管理*******************/
        Task<List<FileVersionDto>> GetVersionList(Guid id);
        Task<bool> CreateFileVersion(FileVersionCreateDto input);
        Task<bool> DeleteFileVersion(Guid id);
        Task<bool> SelectNewVersion(FileVersionInputDto input);
    }
}