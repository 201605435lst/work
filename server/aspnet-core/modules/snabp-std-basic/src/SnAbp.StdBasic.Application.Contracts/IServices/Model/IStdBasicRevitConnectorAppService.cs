using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicRevitConnectorAppService : IApplicationService
    {
        //Task<PagedResultDto<ModelFileRltConnectorDto>> EditList(ModelFileRltConnectorCreateDto input);
        Task<bool> Get(Guid id, string name);

        Task<ModelFileRltConnectorDto> Create(ModelFileRltConnectorCreateDto input);

        Task<bool> Delete(Guid id);

        Task<ModelFileRltConnectorDto> Update(ModelFileRltConnectorDto input);

        Task<PagedResultDto<ModelFileRltConnectorDto>> GetListByModelFileId(ModelFileRltConnectorSearchDto input);

        Task Upload([FromForm] ImportData input, Guid modelFileId);

        Task<Stream> Export(RevitConnectorData input);
    }
}
