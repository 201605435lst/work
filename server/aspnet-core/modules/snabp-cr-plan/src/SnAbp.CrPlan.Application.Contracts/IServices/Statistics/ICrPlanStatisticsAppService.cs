using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.Statistics
{
    //统计报表接口
    public interface ICrPlanStatisticsAppService : IApplicationService
    {
        /// <summary>
        /// 获取年表完成进度
        /// </summary>
        /// <returns></returns>
        Task<List<YearPlanFinishProgressDto>> GetYearPlanProgress(YearMonthPlanInputSearchDto input);


        /// <summary>
        /// 获取指定组织机构，指定设备类型下的完成进度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<RepairGroupFinishInfo>> GetRepairGroupFinishData(RepairGroupInputSearchDto input);

        /// <summary>
        /// 获取饼状图数据
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        Task<GetPieStatisticalDto> GetPieStatistical(YearMonthPlanInputSearchDto input);

        Task<List<GetEquipmentStatisticDto>> GetEquipmentStatistics(SearchEquipmentDto input);
    }
}
