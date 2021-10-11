using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Bpm.IServices
{
    public interface IBpmWorkflowAppService : IApplicationService
    {

        /// <summary>
        /// 获取当前登录用户节点的表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> Get([Required] Guid id);
        /// <summary>
        /// 获取工作流的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkflowDto> GetWorkflow(Guid id);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WorkflowSimpleDto>> GetList(WorkflowSearchInputDto input);


        /// <summary>
        /// 发起
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> Create(WorkflowCreateDto input);


        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> Process(WorkflowProcessDto input);

        /// <summary>
        /// 判断当前节点是否可以回退流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CanReturnStep(WorkflowProcessDto input);


        Task<Guid> GetWorkflowTemplateId([Required] Guid id);

        Task<IOrderedEnumerable<WorkflowImformationDto>> GetWorkflowData(Guid id);

        Task<GetTotalSimpleDto> GetTotal(WorkflowSearchInputDto input);

        Task<List<string>> GetWorkFlowNames();

        Task<bool> OneTouchArrove(string opinion, bool isPass, List<Guid> workflowIds);
    }
}
