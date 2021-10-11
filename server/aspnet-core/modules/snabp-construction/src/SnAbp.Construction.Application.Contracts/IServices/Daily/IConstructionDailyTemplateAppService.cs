using SnAbp.Construction.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

/************************************************************************************
*命名空间：SnAbp.Construction.IServices.Daily
*文件名：IConstructionDailyTemplateAppService
*创建人： liushengtao
*创建时间：2021/7/30 13:42:06
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.IServices.Daily
{
   public  interface IConstructionDailyTemplateAppService : IApplicationService
    {
        /// <summary>
        /// 获取施工日志模板详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DailyTemplateDto> Get(Guid id);
        /// <summary>
        /// 施工日志模板列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DailyTemplateDto>> GetList(DailyTemplateSearchDto input);
        /// <summary>
        /// 新建施工日志模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Create(DailyTemplateSimpleDto input);
        /// <summary>
        /// 更新施工日志模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(DailyTemplateSimpleDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
        /// <summary>
        /// 设置默认模板
        /// </summary>
        Task<bool> SetDefault(Guid id);
    }
}
