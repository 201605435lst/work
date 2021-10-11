using SnAbp.Technology.Dtos;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace SnAbp.Technology.IServices
{
  public interface IConstructInterfaceAppService : IApplicationService
    {
        Task<ConstructInterfaceDto> Create(ConstructInterfaceCreateDto input);
        Task<ConstructInterfaceDto> Update(ConstructInterfaceUpdateDto input);
        Task<PagedResultDto<ConstructInterfaceDto>> GetList(ConstructInterfaceSearchDto input);
        Task<ConstructInterfaceDto> Get(Guid id);
        Task<Stream> Export(ConstructInterfaceExportDto input);
        Task Upload([FromForm] ConstructInterfaceUploadDto input);
    }
}
