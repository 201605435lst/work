using SnAbp.Oa.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Oa.IServices
{
    public interface IOaDutyScheduleAppService:IApplicationService 
    {
        /// <summary>
        /// 获取单个值班记录信息表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DutyScheduleDto> Get(Guid id);
        /// <summary>
        /// 获取值班记录信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<DutyScheduleDto>> GetList(DutyScheduleSearchDto input);
        /// <summary>
        /// 创建值班信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DutyScheduleDto> Create(DutyScheduleCreateDto input);
        /// <summary>
        /// 修改值班信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DutyScheduleDto> Update(DutyScheduleUpdateDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
