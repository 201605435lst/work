/**********************************************************************
*******命名空间： Volo.Abp.Identity
*******接口名称： IIdentityOrganizationAppService
*******接口说明： 组织结构结构
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/8/5 10:58:31
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using Microsoft.AspNetCore.Mvc;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Dtos;

namespace SnAbp.Identity
{
    public interface IIdentityOrganizationAppService : IApplicationService
    {

        Task<PagedResultDto<OrganizationDto>> GetList(OrganizationGetListDto  input);

        Task<OrganizationDto> CreateAsync(OrganizationInputDto input);

        Task<OrganizationDto> GetAsync(Guid id);

        Task<OrganizationDto> UpdateAsync(OrganizationUpdateDto input);       
        Task<bool> BatchUpdateTypeAsync(OrganizationBatchUpdateTypeDto input);

        Task DeleteAsync(Guid id);

        Task Import([FromForm] ImportData input);

        Task<List<Guid>> GetLoginUserOrganizationIds();

        Task<List<OrganizationDto>> GetCurrentUserOrganizations(Guid ?ids);

        Task<bool> HasUser(Guid id);

        Task<Stream> Export(OrganizationData input);
    }
}
