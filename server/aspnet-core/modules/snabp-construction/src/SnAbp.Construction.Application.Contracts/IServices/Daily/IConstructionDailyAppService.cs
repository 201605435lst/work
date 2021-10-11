using SnAbp.Construction.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

/************************************************************************************
*命名空间：SnAbp.Construction.IServices.Daily
*文件名：IConstructionDailyAppService
*创建人： liushengtao
*创建时间：2021/7/21 14:12:55
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.IServices.Daily
{
   public  interface IConstructionDailyAppService: IApplicationService
    {
        /// <summary>
        /// 获取施工日志详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DailyDto> Get(Guid id);
        /// <summary>
        /// 施工日志列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DailyDto>> GetList(DailySearchDto input);
        /// <summary>
        /// 新建施工日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Create(DailyCreateDto input);
        /// <summary>
        /// 更新施工日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DailyDto> Update(DailyUpdateDto input);
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
        /// <summary>
        /// 统计已完成的施工量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> GetDailyRltPlanMaterial(Guid id);
    }
}
