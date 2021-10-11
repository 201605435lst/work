using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Dtos.Test;
using SnAbp.Bpm.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Bpm.IServices
{
    public interface IBpmWorkflowTemplateAppService : IApplicationService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> Get([Required]Guid id);


        /// <summary>
        /// 获取首次发起表单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WorkflowDetailDto> GetWorkflowForInitial([Required] Guid id);


        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<WorkflowTemplateDto>> GetList(WorkflowTemplateSearchInputDto input);


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> Create(WorkflowTemplateCreateDto input);





        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> Update(WorkflowTemplateUpdateDto input);


        /// <summary>
        /// 编辑流程模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> UpdateFlowTemplate(WorkflowTemplateUpdateFlowTemplateInputDto input);


        /// <summary>
        /// 编辑表单模板和流程模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> UpdateFormTemplateAndFlowTemplate(WorkflowTemplateUpdateFormTemplateAndFlowTemplateInputDto input);


        /// <summary>
        /// 编辑发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> UpdatePublishState(WorkflowTemplateChangePublishStateInputDto input);


        /// <summary>
        /// 更新发布对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowTemplateDetailDto> UpdateMembers(WorkflowTemplateUpdateMembersDto input);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        /// <summary>
        ///分组信息获取
        /// </summary>
        /// <returns></returns>
        Task<List<WorkflowTemplateGroupDetailDto>> GetGroupList();
        /// <summary>
        /// 分组信息创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CreateGroup(WorkflowTemplateGroupCreateDto input);
        /// <summary>
        /// 分组信息更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateGroup(WorkflowTemplateGroupCreateDto input);
        /// <summary>
        /// 分组信息删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteGroup(Guid id);
    }
}
