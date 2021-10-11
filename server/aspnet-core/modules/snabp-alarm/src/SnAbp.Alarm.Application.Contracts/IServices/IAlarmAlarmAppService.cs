using Microsoft.AspNetCore.Mvc;
using SnAbp.Alarm.Dtos;
using SnAbp.Resource.Dtos;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Alarm.IServices
{
    public interface IAlarmAlarmAppService : IApplicationService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AlarmDto>> GetList(AlarmGetListDto input);

        /// <summary>
        /// 添加告警
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AlarmDto> Create(AlarmCreateDto input);


        /// <summary>
        /// 导入第三方告警设备id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ImportEquipmentId([FromForm] ImportData input);


        /// <summary>
        /// 根据告警设备 Ids 查询数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<AlarmSimple>> GetAlarmEquipmentBindIdsByIds(List<RealAlarmVaryInfo> realAlarmVaryInfos);
    }
}
