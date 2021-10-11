using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicWorkAttentionAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个事项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkAttentionDto> Get(Guid id);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WorkAttentionDto>> GetList(WorkAttentionSearchDto input);
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkAttentionDto> Create(WorkAttentionCreateDto input);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkAttentionDto> Update(WorkAttentionUpdateDto input);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
