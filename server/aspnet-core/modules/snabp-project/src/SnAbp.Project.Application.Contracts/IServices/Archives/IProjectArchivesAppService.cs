using Microsoft.AspNetCore.Mvc;
using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.IServices
{
   public interface IProjectArchivesAppService : IApplicationService
    {
        Task<ArchivesDto> Create(ArchivesCreateDto input);
        Task<bool> Delete(Guid id);
        Task<ArchivesDto> Update(ArchivesUpdateDto input);
        Task<ArchivesDto> Get(Guid id);
        Task<PagedResultDto<ArchivesDto>> GetList(ArchivesSearchDto input);
        Task<Stream> Export(ArchivesExportDto input);
        Task Upload([FromForm] ArchivesUploadDto input);
    }
}
