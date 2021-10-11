using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using SnAbp.Utils.DataImport;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicStandardEquipmentAppService : IApplicationService
    {
        /// <summary>
        /// 获得标准设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StandardEquipmentDetailDto> Get(Guid id);

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<StandardEquipmentDto>> GetList(StandardEquipmentSearchInputDto input);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StandardEquipmentDto> Create(StandardEquipmentCreateDto input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StandardEquipmentDto> Update(StandardEquipmentUpdateDto input);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<string> Import([FromForm] ImportData input);

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Upload([FromForm] ImportData input);

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> Export(ModelData input);
    }
}
