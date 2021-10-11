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
    public interface IStdBasicComputerCodeAppService : IApplicationService
    {
        Task<PagedResultDto<ComputerCodeDto>> GetList(ComputerCodeGetListDto input);
        Task<ComputerCodeDto> Get(Guid id);
        Task<ComputerCodeDto> Create(ComputerCodeCreateDto input);
        Task<ComputerCodeDto> Update(ComputerCodeUpdateDto input);
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input);
        Task<Stream> Export(ComputerCodeData input);
    }
}
