/**********************************************************************
*******命名空间： SnAbp.Technology.IServices
*******接口名称： IMaterialAppService
*******接口说明：  材料管理服务1
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 6/21/2021 1:47:54 PM
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @Easten 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Technology.Dtos;
using SnAbp.Technology.Dtos.Material;

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
    /// 材料管理相关接口，实现材料管理、用料计划管理等
    /// </summary>
    public interface IMaterialAppService:IApplicationService
    {
        // 材料 维护相关方法
        Task<PagedResultDto<MaterialDto>> GetList(MaterialSearchDto input);
        Task<List<MaterialDto>> GetAllList(Guid typeId);
        Task<bool> Create(MaterialCreateDto input);
        Task<bool> Update(MaterialCreateDto input);
        Task<bool> Delete(Guid id);
        /// <summary>
        /// 数据同步   
        /// </summary>
        /// <returns></returns>
        Task Synchronize(string taskKey);

        // 导出材料信息
        Task<Stream> Export(List<Guid> ids);
        // 用料计划相关方法
        Task<PagedResultDto<MaterialPlanDto>> GetPlanList(MaterialPlanSearchDto input);
        Task<bool> PlanCreate(MaterialPlanCreateDto input);
        Task<bool> PlanUpdate(MaterialPlanCreateDto input);
        Task<bool> PlanDelete(Guid id);
    }
}
