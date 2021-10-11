/**********************************************************************
*******命名空间： SnAbp.File.IServices
*******接口名称： IFileFileManagerAppService
*******接口说明： 除了管理类服务外的其他的服务接口
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/15 9:18:30
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.File.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.File.IServices
{
    public interface IFileFileManagerAppService: IApplicationService
    {
        Task<PagedResultDto<ResourceDto>> Get(ResourceSearchInput input);
        Task<string> GetEndPoint();
        Task<List<OrganizationTreeDto>> GetOrganizationTreeList();
        Task<Guid> GetOrganizationId(OrganizationInputDto input);
        Task<List<OrganizationTreeDto>> GetShareCenterTreeList();
        Task<PagedResultDto<ResourceDto>> GetResourceList(ResourceInputDto input);
        Task<List<MineTreeDto>> GetMineTreeList();
        Task<ResourcePermissionDto> GetResourcePermission(ResourcePermissionInputDto input);
        Task<ResourcePermissionDto> GetResourceShare(ResourcePermissionInputDto input);
        Task<bool> CreateResourceTag(ResourceTagCreateDto input);
        Task<bool> CreateFileMove(FileOperationDto input);
        Task<bool> CreateFileCopy(FileOperationDto input);
        Task<bool> Restore(ResourceRestoreDto input);
        Task<bool> SetResourcePermission(ResourcePermissionCreateDto input);
        Task<bool> SetResourceShare(ResourcePermissionCreateDto input);
        Task<bool> PublishResource(ResourceRestoreDto input);
        Task<bool> Delete(ResourceDeleteDto input);
        Task<bool> DeleteAll();
    }
}
