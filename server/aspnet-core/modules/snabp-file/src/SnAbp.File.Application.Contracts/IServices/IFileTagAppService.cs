/**********************************************************************
*******命名空间： SnAbp.File.IServices
*******类 名 称： ITagAppService
*******类 说 明： 资源标签管理服务接口
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 16:08:40
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
    public interface IFileTagAppService : IApplicationService
    {
        Task<bool> Create(FileTagCreateDto input);
        Task<bool> Update(FileTagUpdateDto input);
        Task<bool> Delete(Guid id);
        Task<List<FileTagDto>> GetList(Guid id);
        /// <summary>
        /// 根据资源id获取对应的标签id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Guid>> GetTagIds(ResourceType type,Guid id);
    }
}