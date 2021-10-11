using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicManufactureAppService : IApplicationService
    {
        Task<ManufacturerDto> Get(Guid id);
        /// <summary>
        /// 根据产品主键获得已关联的厂家列表
        /// </summary>
        Task<List<ManufacturerDto>> GetListByProductId(Guid productId);
        Task<PagedResultDto<ManufacturerSimpleDto>> GetList(ManufacturerGetListDto input);
        //Task<PagedResultDto<ManufacturerSimpleDto>> GetByParentId(ManufacturerSearchDto input);
        Task<ManufacturerDto> Create(ManufacturerCreateDto input);
        Task<ManufacturerDto> Update(ManufacturerUpdateDto input);
        Task Upload([FromForm]ImportData input);
        Task<bool> Delete(Guid id);
        Task<Stream> Export(ManufacturerData input);
    }
}
