using Microsoft.AspNetCore.Http;
using SnAbp.Bpm.IServices;
using SnAbp.File.Dtos;
using SnAbp.File.IServices;
using SnAbp.FileApprove.Dto;
using SnAbp.FileApprove.Entities;
using SnAbp.FileApprove.Enums;
using SnAbp.FileApprove.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;

/************************************************************************************
*命名空间：SnAbp.FileApprove.Service
*文件名：FileApproveFileApproveService
*创建人： liushengtao
*创建时间：2021/9/1 9:50:24
*描述：
*
***********************************************************************/
namespace SnAbp.FileApprove.Service
{
    public class FileApproveFileApproveService : FileApproveAppService, IFileApproveFileApproveService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly IRepository<FileApprove, Guid> _fileApproveRepository;
        readonly IRepository<File.Entities.File, Guid> _fileRepository;
        readonly IRepository<FileApproveRltFlow, Guid> _fileApproveRltFlowRepository;
        readonly ISingleFlowProcessService _singleFlowProcessService;
        readonly IFileFileManagerAppService _iFileFileManagerAppService;

        readonly IUnitOfWorkManager _unitOfWork;
        public FileApproveFileApproveService(
            IFileFileManagerAppService iFileFileManagerAppService,
            IGuidGenerator guidGenerator,
            IRepository<File.Entities.File, Guid> fileRepository,
               IHttpContextAccessor httpContextAccessor,
                IRepository<FileApprove, Guid> fileApproveRepository,
                IRepository<FileApproveRltFlow, Guid> fileApproveRltFlowRepository,
                  IUnitOfWorkManager unitOfWork,
                  ISingleFlowProcessService singleFlowProcessService = null
                )
        {
            _iFileFileManagerAppService = iFileFileManagerAppService;
            _fileRepository = fileRepository;
            _guidGenerator = guidGenerator;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _fileApproveRltFlowRepository = fileApproveRltFlowRepository;
            _fileApproveRepository = fileApproveRepository;
            _singleFlowProcessService = singleFlowProcessService;
        }
        public Task<FileApproveDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");
            var result = _fileApproveRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (result == null) throw new UserFriendlyException("当前数据不存在");
            var res = ObjectMapper.Map<FileApprove, FileApproveDto>(result);
            return Task.FromResult(res);
        }
        public async Task<FileApproveDto> Create(FileApproveSimpleDto input)
        {
            var workflow = await _singleFlowProcessService.CreateSingleWorkFlow((Guid)input.WorkflowTemplateId);
            var result = _fileApproveRepository.WithDetails().Where(x => x.FileId == input.FileId).FirstOrDefault();
            if (result!=null)
            {
                await _fileApproveRepository.DeleteAsync(result.Id);
            }
            var fileApprove = new FileApprove();
            ObjectMapper.Map(input, fileApprove);
            //1、保存基本信息
            fileApprove.SetId(_guidGenerator.Create());
            fileApprove.WorkflowId = workflow.Id;
            fileApprove.Status = FileApprovalStatus.OnReview;// 审核中
            await _fileApproveRepository.InsertAsync(fileApprove);
            var fileApproveDto = ObjectMapper.Map<FileApprove, FileApproveDto>(fileApprove);
            return fileApproveDto;
        }
        [UnitOfWork]
        public async Task<bool> Process(FileApproveProcessDto input)
        {
            var fileApprove = _fileApproveRepository.Where(x => x.FileId == input.FileId).FirstOrDefault();
            var workflowId = fileApprove.WorkflowId.GetValueOrDefault();
            Bpm.Dtos.WorkflowDetailDto dto = null;
            using (var uow = _unitOfWork.Begin(true, false))
            {
                if (input.Status == FileApprovalStatus.Pass)
                {
                    dto = await _singleFlowProcessService.Approved(workflowId, input.Content, CurrentUser.Id);
                }
                else if (input.Status == FileApprovalStatus.UnPass)
                {
                    dto = await _singleFlowProcessService.Stopped(workflowId, input.Content, CurrentUser.Id);
                }
                else
                {
                    throw new UserFriendlyException("流程处理异常");
                }
                await uow.CompleteAsync();
            }
            // 更新当前 流程的状态
            var workflow = _singleFlowProcessService.GetWorkflowById(workflowId).Result;
            switch (workflow.State)
            {
                case Bpm.WorkflowState.Waiting:
                    fileApprove.Status = FileApprovalStatus.OnReview;
                    break;
                case Bpm.WorkflowState.Finished:
                    fileApprove.Status = FileApprovalStatus.Pass;
                    break;
                case Bpm.WorkflowState.Rejected:
                    fileApprove.Status = FileApprovalStatus.UnPass;
                    break;
                default:
                    break;
            }
            if (workflow.WorkflowDatas.Any())
            {
                var data = workflow.WorkflowDatas.Where(a => a.StepState != null)
                    .OrderByDescending(a => a.CreationTime)
                    .FirstOrDefault();
                if (data != null && (data.StepState == Bpm.WorkflowStepState.Stopped || data.StepState == Bpm.WorkflowStepState.Rejected))
                {
                    fileApprove.Status = FileApprovalStatus.UnPass;
                }
            }
            await _fileApproveRepository.UpdateAsync(fileApprove);
            var fileApproveRltFlow = new FileApproveRltFlow(GuidGenerator.Create());
            fileApproveRltFlow.Content = input.Content;
            fileApproveRltFlow.FileApproveId = fileApprove.Id;
            fileApproveRltFlow.WorkFlowId = workflowId;
            fileApproveRltFlow.State = workflow.State;
            await _fileApproveRltFlowRepository.InsertAsync(fileApproveRltFlow);
            return true;
        }
        public Task<FileApproveDto> Update(FileApproveSimpleDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<FileRltFileApproveDto>> GetResourceList(ResourceInputDto input)
        {
            var pageResult = new PagedResultDto<FileRltFileApproveDto>();
            var fileRltFileApproveDto = new List<FileRltFileApproveDto>();
            var file = await _iFileFileManagerAppService.GetResourceList(input);
            fileRltFileApproveDto = ObjectMapper.Map<List<ResourceDto>, List<FileRltFileApproveDto>>(file?.Items.ToList());
            foreach (var item in fileRltFileApproveDto)
            {
                var result = _fileApproveRepository.WithDetails().Where(x => x.FileId == item.Id).FirstOrDefault();
                if (result == null) continue;
                item.WorkflowId = (Guid)result.WorkflowId;
            }
            if (input.IsApprove)
            {
                var list = new List<FileRltFileApproveDto>();
                if (input.Approval)
                {
                    if (input.Waiting)
                    {
                        // 获取待我审批的数据
                        foreach (var item in fileRltFileApproveDto)
                        {
                            if (item.WorkflowId == null || item.WorkflowId == Guid.Empty) continue;
                            if (await _singleFlowProcessService.IsWaitingMyApproval(item.WorkflowId))
                                list.Add(item);
                        }
                    }
                    else
                    {
                        // 获取我已审批的数据
                        var workflowIds = await _singleFlowProcessService.GetMyApprovaledWorkflow();
                        if (workflowIds.Any())
                        {
                            list = fileRltFileApproveDto.Where(a => workflowIds.Contains(a.WorkflowId)).ToList();

                        }
                    }
                }
                else
                {
                    list = fileRltFileApproveDto;
                }

                var res = CalculateMaterialPlanState(list).ToList();
                pageResult.TotalCount = res.Count;
                pageResult.Items = res
                    .Skip(input.Page)
                    .Take(input.Size)
                    .ToList();
            }
            else
            {
                pageResult.TotalCount = file.TotalCount;
                pageResult.Items = CalculateMaterialPlanState(fileRltFileApproveDto).ToList();
            }

            return pageResult;
        }

        /// <summary>
        /// 计算用料计划的流程状态
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<FileRltFileApproveDto> CalculateMaterialPlanState(List<FileRltFileApproveDto> list)
        {
            var result = new List<FileRltFileApproveDto>();
            foreach (var item in list)
            {
                if (item.WorkflowId != null && item.WorkflowId != Guid.Empty)
                {
                    var workflow = _singleFlowProcessService.GetWorkflowById(item.WorkflowId).Result;
                    switch (workflow.State)
                    {
                        case Bpm.WorkflowState.Waiting:
                            item.Status = FileApprovalStatus.OnReview;
                            break;
                        case Bpm.WorkflowState.Finished:
                            item.Status = FileApprovalStatus.Pass;
                            break;
                        case Bpm.WorkflowState.Rejected:
                            item.Status = FileApprovalStatus.UnPass;
                            break;
                        default:
                            break;
                    }
                    //根据审批数据进行判断当前的状态
                    if (workflow.WorkflowDatas.Any())
                    {
                        var data = workflow.WorkflowDatas.Where(a => a.StepState != null)
                            .OrderByDescending(a => a.CreationTime)
                            .FirstOrDefault();
                        if (data != null && (data.StepState == Bpm.WorkflowStepState.Stopped || data.StepState == Bpm.WorkflowStepState.Rejected))
                        {
                            item.Status = FileApprovalStatus.UnPass;
                        }
                    }
                }
                result.Add(item);
            }
            return result;
        }

    }
}
