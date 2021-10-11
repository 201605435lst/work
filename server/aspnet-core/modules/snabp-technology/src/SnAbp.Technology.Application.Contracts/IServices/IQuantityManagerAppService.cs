/**********************************************************************
*******命名空间： SnAbp.Technology.IServices
*******接口名称： QuantityManagerAppService
*******接口说明： 工程量管理服务接口，包括工程量统计、用料计划管理等任务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/23/2021 3:18:05 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
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
    /// <summary>
    /// 工程量管理服务
    /// </summary>
    public interface IQuantityManagerAppService : IApplicationService
    {
        Task<PagedResultDto<QuantitiesDto>> GetList(QuantitiesSearchDto input);
        Task<Stream> Export(List<Guid> ids);
        Task<bool> CreateMaterialPlan(MaterialPlanCreateDto input);

        /// <summary>
        /// 获取所有的工程量数据 (张哥这是我加的,我要用……)
        /// </summary>
        /// <returns></returns>
        Task<List<QuantitiesDto>> GetAllList();

    }
}
