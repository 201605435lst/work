using Microsoft.Extensions.Logging;
using Quartz;
using SnAbp.CrPlan.Dtos;
using SnAbp.CrPlan.Entities;
using SnAbp.CrPlan.IServices.Statistics;
using SnAbp.CrPlan.Services;
using SnAbp.Identity;
using SnAbp.StdBasic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers.Quartz;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.CrPlan
{
    public class AutoSaveStatisticWorker : QuartzBackgroundWorkerBase
    {
        private ICrPlanStatisticsAppService _crPlanStatisticsAppService;
        private readonly IRepository<StatisticsPieWorker, Guid> statisticsPieWorkers;
        private readonly IRepository<StatisticsEquipmentWorker, Guid> statisticsEquipmentWorkers;

        public AutoSaveStatisticWorker(
           ICrPlanStatisticsAppService crPlanStatisticsAppService,
           IRepository<StatisticsPieWorker, Guid> statisticsPieWorkers,
           IRepository<StatisticsEquipmentWorker, Guid> statisticsEquipmentWorkers
            )
        {
            //AutoRegister = false;
            JobDetail = JobBuilder.Create<AutoSaveStatisticWorker>().WithIdentity(nameof(AutoSaveStatisticWorker)).Build();
            Trigger = TriggerBuilder.Create()
                .WithCronSchedule("0 0/30 * * * ? ")//0 0/30 * * * ? 
                .Build();
            _crPlanStatisticsAppService = crPlanStatisticsAppService;
            this.statisticsPieWorkers = statisticsPieWorkers;
            this.statisticsEquipmentWorkers = statisticsEquipmentWorkers;
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            var nowTime = DateTime.Now;
            //删除当年全部数据
            //await statisticsEquipmentWorkers.DeleteAsync(x => x.Year == nowTime.Year);
            //await statisticsPieWorkers.DeleteAsync(x => x.Year == nowTime.Year);


            var yearDto = new YearMonthPlanInputSearchDto()
            {
                IsLoginFree = true,
                Type = 1,
            };

            var monthDto = new YearMonthPlanInputSearchDto()
            {
                IsLoginFree = true,
                Type = 2,
            };

            for (int i = 0; i < nowTime.Month; i++)
            {
                decimal yearTotalCount = 0;
                decimal yearFinishCount = 0;
                decimal yeatAlterCount = 0;
                decimal yearUnFinishCount;

                decimal monthTotalCount = 0;
                decimal monthFinishCount = 0;
                decimal monthAlterCount = 0;
                decimal momthUnFinishCount;

                var time = DateTime.Now.AddMonths(-i);

                monthDto.Time = time;
                yearDto.Time = time;

                var yearResults = await _crPlanStatisticsAppService.GetYearPlanProgress(yearDto);
                var monthResults = await _crPlanStatisticsAppService.GetYearPlanProgress(monthDto);

                if ((yearResults == null || yearResults.Count == 0) && (monthResults == null || monthResults.Count == 0)) continue;

                foreach (var year in yearResults)
                {
                    //构造各个车间的数据
                    await InsertOrganizationStatistic(1, year, time);

                    //构造总设备数据
                    await InsertEquipmentStatistic(1, year, time);

                    //构造科室总体数据
                    var totalFinishInfo = year.TotalFinishInfo;
                    yearTotalCount += totalFinishInfo.TotalCount;
                    yearFinishCount += totalFinishInfo.FinishCount;
                    yeatAlterCount += totalFinishInfo.AlterCount;
                    //yearUnFinishCount = yearTotalCount - yearFinishCount - yeatAlterCount;
                }
                yearUnFinishCount = yearTotalCount - yearFinishCount - yeatAlterCount;

                var yearStaticsPie = new StatisticsPieWorker(Guid.NewGuid())
                {
                    Changed = (float)yeatAlterCount,
                    Finshed = (float)yearFinishCount,
                    UnFinshed = (float)yearUnFinishCount,
                    Type = 1,
                    Year = time.Year,
                    Month = time.Month
                };

                foreach (var month in monthResults)
                {
                    //构造车间数据
                    await InsertOrganizationStatistic(2, month, time);

                    //构造总设备数据
                    await InsertEquipmentStatistic(2, month, time);

                    //构造总体数据
                    var totalFinishInfo = month.TotalFinishInfo;
                    monthTotalCount += totalFinishInfo.TotalCount;
                    monthFinishCount += totalFinishInfo.FinishCount;
                    monthAlterCount += totalFinishInfo.AlterCount;
                }

                momthUnFinishCount = monthTotalCount - monthFinishCount - monthAlterCount;

                var monthStaticsPie = new StatisticsPieWorker(Guid.NewGuid())
                {
                    Changed = (float)monthAlterCount,
                    Finshed = (float)monthFinishCount,
                    UnFinshed = (float)momthUnFinishCount,
                    Type = 2,
                    Year = time.Year,
                    Month = time.Month
                };

                await statisticsPieWorkers.DeleteAsync(x => x.Year == time.Year && x.Month == time.Month && x.OrgizationName == null);
                await statisticsPieWorkers.InsertAsync(yearStaticsPie);
                await statisticsPieWorkers.InsertAsync(monthStaticsPie);
            }

            Logger.LogError("执行成功");
        }

        /// <summary>
        /// 插入车间数据统计
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task<bool> InsertOrganizationStatistic(int type, YearPlanFinishProgressDto dto, DateTime time)
        {
            //构造车间数据
            var monthStaticsHistogram = new StatisticsPieWorker(Guid.NewGuid())
            {
                Changed = (float)dto.TotalFinishInfo.AlterPercent,
                Finshed = (float)dto.TotalFinishInfo.FinishPercent,
                UnFinshed = (float)(100 - dto.TotalFinishInfo.FinishPercent - dto.TotalFinishInfo.AlterPercent),
                Type = type,
                Year = time.Year,
                Month = time.Month,
                OrgizationName = dto.OrgName,
                OrgizationId = dto.OrgId
            };
            await statisticsPieWorkers.DeleteAsync(x => x.Year == time.Year && x.Month == time.Month && x.OrgizationName == dto.OrgName && x.Type == type);
            await statisticsPieWorkers.InsertAsync(monthStaticsHistogram);
            return true;
        }

        /// <summary>
        /// 插入设备数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dto"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private async Task<bool> InsertEquipmentStatistic(int type, YearPlanFinishProgressDto dto, DateTime time)
        {
            foreach (var item in dto.RepairGroupFinishInfos)
            {
                var equipmentStatistic = new StatisticsEquipmentWorker(Guid.NewGuid())
                {
                    Type = type,
                    Year = time.Year,
                    Month = time.Month,
                    OrgizationName = dto.OrgName,
                    OrgizationId = dto.OrgId,
                    GroupName = item.Name,
                    Finshed = (float)item.FinishInfo.FinishPercent,
                    UnFinshed = (float)(100 - item.FinishInfo.FinishPercent - item.FinishInfo.AlterPercent),
                    Changed = (float)item.FinishInfo.AlterPercent
                };
                await statisticsEquipmentWorkers
                    .DeleteAsync(x => x.Year == time.Year && x.Month == time.Month && x.GroupName == item.Name && x.Type == type && x.OrgizationId == dto.OrgId);
                await statisticsEquipmentWorkers.InsertAsync(equipmentStatistic);
            }
            return true;
        }

    }
}
