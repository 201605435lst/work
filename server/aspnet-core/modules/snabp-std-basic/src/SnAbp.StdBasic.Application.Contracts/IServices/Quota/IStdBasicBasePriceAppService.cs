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
    public interface IStdBasicBasePriceAppService : IApplicationService
    {
        Task<PagedResultDto<BasePriceDto>> GetList(BasePriceGetListDto input);
        Task<BasePriceDto> Get(Guid id);
        Task<BasePriceDto> Create(BasePriceCreateDto input);
        Task<BasePriceDto> Update(BasePriceUpdateDto input);
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input,Guid id);
        Task<Stream> Export(BasePriceData input);
    }
}
