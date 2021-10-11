using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicQuotaAppService : IApplicationService
    {
        Task<PagedResultDto<QuotaDto>> GetList(QuotaGetListDto input);
        Task<QuotaDto> Get(Guid id);
        Task<QuotaDto> Create(QuotaCreateDto input);
        Task<QuotaDto> Update(QuotaUpdateDto input);
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input);
        Task<Stream> Export(QuotaData input);
    }
}
