
using Microsoft.AspNetCore.Mvc;
using SnAbp.Basic.Dtos;
using SnAbp.Basic.Dtos.Export;
using SnAbp.Basic.Dtos.Import;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Basic.IServices
{
    public interface IBasicStationAppService : IApplicationService
    {
        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StationDetailDto> Get(Guid id);

        /// <summary>
        /// 根据条件获取站点
        /// </summary>
        /// <param name="staName">站点名称</param>
        /// <param name="repairTeamId">维护班组Guid</param>
        /// <param name="belongRaId">所属线路Guid</param>
        /// <returns></returns>
        Task<PagedResultDto<StationSimpleDto>> GetList(StationSearchDto input);

        /// <summary>
        /// 获取极简数据
        /// </summary>
        /// <returns></returns>
        Task<List<StationVerySimpleDto>> GetSimpleList(StationSimpleSearchDto input);

        /// <summary>
        /// 根据线路主键获取站点列表
        /// </summary>
        Task<List<StationDto>> GetListByRailwayId(Guid railwayId);

        /// <summary>
        /// 根据组织机构（维护车间）获取 线路-站点级联数据
        /// </summary>
        /// <param name="id">所属组织机构</param>
        /// <returns></returns>
        Task<List<StationRailDto>> GetCascaderListWithOrg(StationCascaderDto input);
        
        /// <summary>
        /// 根据组织机构获取车站
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        Task<List<StationDto>> GetListByOrg(Guid orgId);

        /// <summary>
        /// 添加站点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StationSimpleDto> Create(StationInputDto input);

        /// <summary>
        /// 编辑站点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StationSimpleDto> Update(StationUpdateDto input);

        /// <summary>
        /// 删除站点
        /// </summary>
        /// <param name="id">站点Guid</param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        /// 导入站点excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task Upload([FromForm] ImportData input);

        Task<Stream> Export(StationExportData input);
    }
}
