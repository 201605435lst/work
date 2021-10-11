using SnAbp.Construction.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Construction.IServices
{
    public interface IConstructionDispatchAppService : IApplicationService
    {
        /// <summary>
        /// 获取派工详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DispatchDto> Get(Guid id);
        /// <summary>
        /// 派工列表获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DispatchDto>> GetList(DispatchSearchDto input);
        /// <summary>
        /// 新建派工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Create(DispatchCreateDto input);
        /// <summary>
        /// 更新派工
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(DispatchUpdateDto input);
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> ids);
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Stream> Export(Guid id);
        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="id">派工单id</param>
        /// <param name="workFlowId">审批流程id </param>
        /// <returns></returns>
        Task<bool> ForSubmit(Guid id, Guid workFlowId);
        /// <summary>
        /// 审批流程操作
        /// </summary>
        /// <returns></returns>
        Task<bool> Process(DispatchProcessDto input);
    }
}
