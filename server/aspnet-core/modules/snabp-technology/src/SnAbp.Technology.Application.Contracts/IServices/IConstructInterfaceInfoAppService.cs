using SnAbp.Technology.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Technology.IServices
{
   public interface IConstructInterfaceInfoAppService : IApplicationService
    {
        Task<bool> Create(ConstructInterfaceInfoCreateDto input);
        Task<bool> Update(ConstructInterfaceInfoUpdateDto input);
        Task<ConstructInterfaceInfoDto> Get(Guid id);
        Task<bool> InterfanceReform(ConstructInterfaceInfoReformDto input);
        Task<Stream> Export(ConstructInterfaceInfoExportDto input);
        Task<List<ConstructInterfaceInfoDto>> GetInterfanceReform(ConstructInterfaceInfoReformSimpleDto input);
    }
}

