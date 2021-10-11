using SnAbp.Basic.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using SnAbp.Utils.DataImport;
using SnAbp.Basic.Dtos.Import;
using System.IO;
using SnAbp.Basic.Dtos.Export;

namespace SnAbp.Basic.IServices
{
    public interface IBasicRailwayAppService : IApplicationService
    {
        /// <summary>
        /// 根据ID获取线路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RailwayDetailDto> Get(Guid id);

        /// <summary>
        /// 根据条件查询线路
        /// </summary>
        /// <param name="raName">线路名称</param>
        /// <param name="staName">途径站点名称</param>
        /// <returns></returns>
        Task<PagedResultDto<RailwaySimpleDto>> GetList(RailwaySearchDto input);

        /// <summary>
        /// 添加线路
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RailwayDto> Create(RailwayCreateDto input);

        /// <summary>
        /// 给线路关联站点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateStations(RelateSta_RaInputDto input);

        /// <summary>
        /// 修改线路信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(RailwayUpdateDto input);

        /// <summary>
        /// 根据ID删除线路
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="files"></param>
        /// <param name="belongOrgCode">所属段编码</param>
        /// <returns></returns>
        Task<string> Upload([FromForm] ImportData input);

        Task<Stream> Export(RailwayExportData input);
    }
}
