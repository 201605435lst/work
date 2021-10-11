using SnAbp.CrPlan.Dto.Statistical;
using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.Statistical
{
    /// <summary>
    /// 智能报表模块接口
    /// </summary>
    public interface ICrPlanStatisticalAppService : IApplicationService
    {
        /// <summary>
        /// 根据用户组织机构Id获取整个段的年计划完成情况统计列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<YearStatisticalDto>> GetYearStatistical(Guid id, string repairTag);

        /// <summary>
        /// 根据查询条件获取整个段的月计划完成情况统计列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<MonthStatisticalDto>> GetMonthStatistical(StatisticalChartsSearchInputDto input);

        /// <summary>
        /// 根据查询条件获取单项完成情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<SingleCompleteDto>> GetSingleStatistical(SingleCompleteSearchInputDto input);

        /// <summary>
        /// 根据查询条件获取设备类型的完成情况统计列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<EquipmentStatisticalDto>> GetEquipmentStatistical(StatisticalChartsSearchInputDto input);



        /// <summary>
        /// 根据查询条件获取计划状态追踪实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PlanStateTrackingDto> GetPlanStateTracking(PlanStateTrackingSearchInputDto input);


        /// <summary>
        /// 根据查询条件获取月计划完成情况列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<MonthCompletionDto>> GetMonthCompletion(MonthFinishSearchInputDto input);

        /// <summary>
        /// 根据查询条件获取月完成率列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        Task<List<MonthCompletionRatesDto>> GetMonthCompletionRates(MonthFinishSearchInputDto input);

        /// <summary>
        /// 根据查询条件获取年表完成率列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<YearCompletionRatesDto>> GetYearCompletionRates(YearFinishSearchInputDto input);
        
        /// <summary>
        ///导出月完成情况 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> ExportMonthCompletion(MonthFinishSearchInputDto input);

        /// <summary>
        /// 月完成率统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Stream> ExprortMonthCompletionRates(MonthFinishSearchInputDto input);

        Task<Stream> ExprortYearCompletionRates(YearFinishSearchInputDto input);

    }
}
