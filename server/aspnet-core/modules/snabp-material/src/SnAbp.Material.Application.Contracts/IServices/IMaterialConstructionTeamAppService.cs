using Microsoft.AspNetCore.Mvc;
using SnAbp.Material.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
   public interface IMaterialConstructionTeamAppService : IApplicationService
    {
        Task<ConstructionTeamDto> Create(ConstructionTeamCreateDto input);
        Task<bool> Delete(Guid Id);
        Task<ConstructionTeamDto> Update(ConstructionTeamUpdateDto input);
        Task<PagedResultDto<ConstructionTeamDto>> GetList(ConstructionTeamSearchDto input);
        Task<ConstructionTeamDto> Get(Guid id);
        Task<Stream> Export(ConstructionTeamExportDto input);
        Task Upload([FromForm] ConstructionTeamUploadDto input);
    }
}