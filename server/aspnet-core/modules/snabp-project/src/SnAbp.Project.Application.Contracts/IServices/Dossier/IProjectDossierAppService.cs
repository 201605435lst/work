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
    public interface IProjectDossierAppService : IApplicationService
    {
        Task<DossierDto> Create(DossierCreateDto input);
        Task<bool> Delete(Guid id);
        Task<DossierDto> Update(DossierUpdateDto input);
        Task<DossierDto> Get(Guid id);
        Task<PagedResultDto<DossierDto>> GetList(DossierSearchDto input);
        Task Upload([FromForm] DossierUploadDto input);
        Task<Stream> Export(DossierExportDto input);
    }
}
