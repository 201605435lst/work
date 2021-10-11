using Microsoft.EntityFrameworkCore;
using SnAbp.Bpm.Dtos;
using SnAbp.Bpm.Entities;
using SnAbp.Bpm.IServices;
using SnAbp.Schedule.Dtos;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using SnAbp.Schedule.IServices;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace SnAbp.Schedule.Services
{
    public class ScheduleScheduleAppService : ScheduleAppService, IScheduleScheduleAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        private readonly IRepository<ScheduleRltSchedule, Guid> _scheduleRltSchedulesRepository;
        private readonly IRepository<ScheduleFlowInfo, Guid> _scheduleFlowInfoRepository;
        private readonly IRepository<ScheduleFlowTemplate, Guid> _scheduleFlowTemplateRepository;
        private readonly IRepository<ProcessTemplate, Guid> _processTemplatesRepository;
        private readonly IRepository<ProjectItemRltProcessTemplate, Guid> _projectItemRltProcessTemplatesRepository;
        private readonly IRepository<WorkflowData, Guid> _workflowDataRepository;
        private readonly IRepository<Workflow, Guid> _workflowsRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        ISingleFlowProcessService _singleFlowProcess;

        public ScheduleScheduleAppService(
            IRepository<Schedule, Guid> scheduleRepository,
            IRepository<ScheduleRltSchedule, Guid> scheduleRltSchedulesRepository,
            IGuidGenerator guidGenerator,
            IRepository<ScheduleFlowInfo, Guid> scheduleFlowInfoRepository,
            IRepository<ScheduleFlowTemplate, Guid> scheduleFlowTemplateRepository,
            IRepository<ProcessTemplate, Guid> processTemplatesRepository,
            IRepository<ProjectItemRltProcessTemplate, Guid> projectItemRltProcessTemplatesRepository,
            IRepository<WorkflowData, Guid> workflowDataRepository,
            IRepository<Workflow, Guid> workflowsRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISingleFlowProcessService singleFlowProcess
            )
        {
            _scheduleRepository = scheduleRepository;
            _scheduleRltSchedulesRepository = scheduleRltSchedulesRepository;
            _guidGenerator = guidGenerator;
            _scheduleFlowInfoRepository = scheduleFlowInfoRepository;
            _scheduleFlowTemplateRepository = scheduleFlowTemplateRepository;
            _processTemplatesRepository = processTemplatesRepository;
            _projectItemRltProcessTemplatesRepository = projectItemRltProcessTemplatesRepository;
            _workflowDataRepository = workflowDataRepository;
            _singleFlowProcess = singleFlowProcess;
            _workflowsRepository = workflowsRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ScheduleDto> Create(ScheduleCreateDto input)
        {
            if (input.ScheduleIdLists == null) //当为单个添加时判断
            {
                if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("任务名称不能为空");
            }
            if (input.ProfessionId == null) throw new UserFriendlyException("所属专业不能为空");
            if (!input.Type.IsIn(WorkType.AutoCompute, WorkType.Milestone, WorkType.Important, WorkType.Unimportant)) throw new UserFriendlyException("请选择工作类型");
            if (input.StartTime != null && input.EndTime != null)
            {
                if (input.EndTime <= input.StartTime)
                {
                    throw new UserFriendlyException("计划完成时间不能在计划开始时间之前！");
                }
            }
            await CheckSameName(input.ParentId, null, input.Name);

            using var unow = _unitOfWorkManager.Begin(true, false);
            Schedule schedule = null;
            if (input.ScheduleIdLists == null)  //单个添加
            {
                schedule = new Schedule(_guidGenerator.Create())
                {
                    ParentId = input.ParentId,
                    ProfessionId = input.ProfessionId,
                    Name = input.Name,
                    Location = input.Location,
                    Type = input.Type,
                    StartTime = input.StartTime,
                    EndTime = input.EndTime,
                    TimeLimit = input.TimeLimit,
                    //ProjectType = input.ProjectType,  改成关联表了
                    State = State.NoStart,
                    IsAuto = input.IsAuto,
                };
                //保存关联前置任务
                schedule.ScheduleRltSchedules = new List<ScheduleRltSchedule>();
                foreach (var front in input.ScheduleRltSchedules)
                {
                    schedule.ScheduleRltSchedules.Add(new ScheduleRltSchedule(_guidGenerator.Create())
                    {
                        ScheduleId = schedule.Id,
                        FrontScheduleId = front.Id
                    });
                }
                //保存关联工程工项
                schedule.ScheduleRltProjectItems = new List<ScheduleRltProjectItem>();
                foreach (var projectItem in input.ScheduleRltProjectItems)
                {
                    schedule.ScheduleRltProjectItems.Add(new ScheduleRltProjectItem(_guidGenerator.Create())
                    {
                        ScheduleId = schedule.Id,
                        ProjectItemId = projectItem.Id,
                    });
                }

                //保存关联设备（模型）
                schedule.ScheduleRltEquipments = new List<ScheduleRltEquipment>();
                foreach (var equipment in input.ScheduleRltEquipments)
                {
                    schedule.ScheduleRltEquipments.Add(new ScheduleRltEquipment(_guidGenerator.Create())
                    {
                        ScheduleId = schedule.Id,
                        EquipmentId = equipment.Id,
                        StartTime = equipment.StartTime,
                        EndTime = equipment.EndTime,
                        Progress = equipment.Progress,
                        State = equipment.State,
                    });
                }

                await _scheduleRepository.InsertAsync(schedule);
                await unow.SaveChangesAsync();

                var oldSchedule = _scheduleRepository.FirstOrDefault(x => x.Id == schedule.Id);
                if (input.WorkflowTemplateId != null && input.WorkflowTemplateId != Guid.Empty) //当为人工审核时
                {
                    // 先创建一个并启动一个计划
                    var workflow = await _singleFlowProcess.CreateSingleWorkFlow((Guid)input.WorkflowTemplateId);
                    oldSchedule.WorkflowTemplateId = input.WorkflowTemplateId;
                    oldSchedule.WorkflowId = workflow.Id;
                }
                else //当为自动审核，则找到已设置的模板，去赋值
                {
                    var scheduleflow = _scheduleFlowTemplateRepository.FirstOrDefault();
                    if (scheduleflow != null)
                    {
                        oldSchedule.WorkflowTemplateId = scheduleflow.WorkflowTemplateId;
                        // 先创建一个并启动一个计划
                        var workflow = await _singleFlowProcess.CreateSingleWorkFlow((Guid)schedule.WorkflowTemplateId);
                        oldSchedule.WorkflowId = workflow.Id;
                    }
                    else
                    {
                        throw new UserFriendlyException("请先设置自动审核流程！");
                    }
                }

                if (input.IsAuto) //如果是自动审核，则自动保存审核信息，且计划审批变为已完成
                {
                    //获取计划关联流程的各个节点的名称及审批人员信息
                    var flowNodes = await GetSingleFlowNodes((Guid)oldSchedule.WorkflowId, schedule.Id);

                    //递归将每个节点审批通过
                    await EverApprovalAsync(flowNodes, oldSchedule.Id);
                    await unow.SaveChangesAsync();
                    oldSchedule.State = State.Finshed;
                }
                await _scheduleRepository.UpdateAsync(oldSchedule);
                await unow.SaveChangesAsync();
                //ApprovalSchedule(schedule, input.WorkflowTemplateId, input.IsAuto);  将操作更新的代码封装成方法去调就会报错，奇怪的一批
            }
            else
            {
                var templates = _processTemplatesRepository.Where(x => input.ScheduleIdLists.Contains(x.Id));

                foreach (var tem in templates) //判断重名
                {
                    await CheckSameName(tem.ParentId, null, tem.Name);
                }

                var listDtos =
                    ObjectMapper.Map<List<ProcessTemplate>, List<ProcessTemplateDto>>(templates.Distinct()
                        .ToList());

                var processTemplates = GuidKeyTreeHelper<ProcessTemplateDto>.GetTree(listDtos);

                var scheduleflow = _scheduleFlowTemplateRepository.FirstOrDefault();
                //递归添加计划
                var scheduleList = new List<Schedule>();
                RecursionAdd(processTemplates, null, input, scheduleflow,scheduleList);
                await unow.SaveChangesAsync();
                foreach (var sche in scheduleList)
                {
                    var oldSchedule = _scheduleRepository.FirstOrDefault(x => x.Id == sche.Id);

                    if (input.WorkflowTemplateId != null && input.WorkflowTemplateId != Guid.Empty) //当为人工审核时
                    {
                        // 先创建一个并启动一个计划
                        var workflow = await _singleFlowProcess.CreateSingleWorkFlow((Guid)input.WorkflowTemplateId);
                        oldSchedule.WorkflowTemplateId = input.WorkflowTemplateId;
                        oldSchedule.WorkflowId = workflow.Id;
                    }
                    else //当为自动审核，则找到已设置的模板，去赋值
                    {
                        if (scheduleflow != null)
                        {
                            oldSchedule.WorkflowTemplateId = scheduleflow.WorkflowTemplateId;
                            // 先创建一个并启动一个计划
                            var workflow = await _singleFlowProcess.CreateSingleWorkFlow((Guid)sche.WorkflowTemplateId);
                            oldSchedule.WorkflowId = workflow.Id;
                        }
                        else
                        {
                            throw new UserFriendlyException("请先设置自动审核流程！");
                        }
                    }
                    if (input.IsAuto) //如果是自动审核，则自动保存审核信息，且计划审批变为已完成
                    {
                        //获取计划关联流程的各个节点的名称及审批人员信息
                        var flowNodes = await GetSingleFlowNodes((Guid)oldSchedule.WorkflowId, sche.Id);

                        //递归将每个节点审批通过
                        await EverApprovalAsync(flowNodes, oldSchedule.Id);
                        await unow.SaveChangesAsync();

                        oldSchedule.State = State.Finshed;
                    }
                    await _scheduleRepository.UpdateAsync(oldSchedule);
                    await unow.SaveChangesAsync();
                }
            }

            return await Task.FromResult(ObjectMapper.Map<Schedule, ScheduleDto>(schedule));
        }

        public Task<ScheduleDto> Get(Guid id)
        {
            if (id == null || Guid.Empty == id) throw new UserFriendlyException("id不能为空");
            var schedule = _scheduleRepository.WithDetails().FirstOrDefault(s => s.Id == id);
            if (schedule == null) throw new UserFriendlyException("当前计划不存在");

            var result = ObjectMapper.Map<Schedule, ScheduleDto>(schedule);

            foreach (var ite in result.ScheduleRltSchedules)
            {
                ite.FrontSchedule = ObjectMapper.Map<Schedule, ScheduleDto>(_scheduleRepository.FirstOrDefault(x => x.Id == ite.FrontScheduleId));
            }

            return Task.FromResult(result);
        }

        public Task<PagedResultDto<ScheduleSimpleDto>> GetByIds(List<Guid> ids)
        {
            var schedules = _scheduleRepository.WithDetails().WhereIf(ids.Any(), x => ids.Contains(x.Id));
            var result = new PagedResultDto<ScheduleSimpleDto>();
            result.TotalCount = schedules.Count();
            result.Items = ObjectMapper.Map<List<Schedule>, List<ScheduleSimpleDto>>(schedules.ToList());
            return Task.FromResult(result);
        }

        public async Task<PagedResultDto<ScheduleDto>> GetList(ScheduleSearchDto input)
        {
            var dto = new List<ScheduleDto>();
            var schedule = _scheduleRepository.WithDetails()
                .Where(x => x.ParentId == input.ParentId) //树状表格条件
                .WhereIf(input.ProfessionId != null && input.ProfessionId != Guid.Empty, x => x.ProfessionId == input.ProfessionId)
                .WhereIf(input.StartTime != null && input.EndTime != null, x => x.StartTime >= input.StartTime && x.EndTime <= input.EndTime)
                .WhereIf(input.State.IsIn(State.Processing, State.Finshed, State.Refused, State.NoStart, State.Stop), x => x.State == input.State)
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Progress.ToString().Contains(input.Keywords))
                .WhereIf(input.NoId != null && input.NoId != Guid.Empty, x => x.Id != input.NoId).ToList();

            dto = ObjectMapper.Map<List<Schedule>, List<ScheduleDto>>(schedule);

            foreach (var item in dto)
            {
                item.Children = item.Children.Count == 0 ? null : new List<ScheduleDto>();
            }

            var result = new PagedResultDto<ScheduleDto>();
            result.TotalCount = dto.Count();
            var list = dto.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            foreach (var item in list)
            {
                foreach (var ite in item.ScheduleRltSchedules)
                {
                    ite.FrontSchedule = ObjectMapper.Map<Schedule, ScheduleDto>(_scheduleRepository.FirstOrDefault(x => x.Id == ite.FrontScheduleId));
                }
                //item.ScheduleRltSchedules = ObjectMapper.Map<List<Schedule>, List<ScheduleRltScheduleDto>>(_scheduleRepository.Where(x => item.ScheduleRltSchedules.Any(y => y.FrontScheduleId == item.Id)).ToList());
            }
            result.Items = list;

            return await Task.FromResult(result);
        }

        public async Task<ScheduleDto> Update(ScheduleUpdateDto input)
        {
            var oldSch = _scheduleRepository.FirstOrDefault(s => s.Id == input.Id);
            if (oldSch == null) throw new UserFriendlyException("当前更新计划不存在");

            if (input.IsMilestone)
            {
                oldSch.Type = WorkType.Milestone;
                await _scheduleRepository.UpdateAsync(oldSch);
                return ObjectMapper.Map<Schedule, ScheduleDto>(oldSch);
            }
            if (input.IsFrontSchedules)
            {
                //清除之前关联任务信息
                await _scheduleRltSchedulesRepository.DeleteAsync(x => x.ScheduleId == input.Id);

                // 重新保存关联任务信息
                oldSch.ScheduleRltSchedules = new List<ScheduleRltSchedule>();
                foreach (var front in input.ScheduleRltSchedules)
                {
                    var srs = new ScheduleRltSchedule(_guidGenerator.Create())
                    {
                        ScheduleId = oldSch.Id,
                        FrontScheduleId = front.Id
                    };
                    await _scheduleRltSchedulesRepository.InsertAsync(srs);
                }
                //await _scheduleRepository.UpdateAsync(oldSch);
                return ObjectMapper.Map<Schedule, ScheduleDto>(oldSch);
            }

            oldSch.ProfessionId = input.ProfessionId;
            oldSch.Name = input.Name;
            oldSch.Location = input.Location;
            oldSch.Type = input.Type;
            oldSch.StartTime = input.StartTime;
            oldSch.EndTime = input.EndTime;
            oldSch.TimeLimit = (double)input.TimeLimit;
            //oldSch.ProjectType = input.ProjectType;
            oldSch.IsAuto = input.IsAuto;

            //清除关联流程


            //清除关联EBS工程工项

            //清除之前关联任务信息
            await _scheduleRltSchedulesRepository.DeleteAsync(x => x.ScheduleId == input.Id);
            // 重新保存关联任务信息
            oldSch.ScheduleRltSchedules = new List<ScheduleRltSchedule>();
            foreach (var front in input.ScheduleRltSchedules)
            {
                oldSch.ScheduleRltSchedules.Add(new ScheduleRltSchedule(_guidGenerator.Create())
                {
                    ScheduleId = oldSch.Id,
                    FrontScheduleId = front.Id
                });
            }
            await _scheduleRepository.UpdateAsync(oldSch);
            return ObjectMapper.Map<Schedule, ScheduleDto>(oldSch);
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");

            await _scheduleRltSchedulesRepository.DeleteAsync(x => x.ScheduleId == id);

            await _scheduleRepository.DeleteAsync(id);

            return true;
        }

        public Task<Stream> Export(EduceScheduleDto input)
        {
            var schedules = _scheduleRepository.WithDetails().Where(x => input.ScheduleIds.Contains(x.Id)).ToList();
            var list = schedules.OrderBy(x => x.CreationTime).ToList();
            Stream stream = null;
            byte[] sbuf;
            var dt = (DataTable)null;
            var dataColumn = (DataColumn)null;
            var dataRow = (DataRow)null;
            dt = new DataTable();
            //添加表头
            var enumValues = Enum.GetValues(typeof(ExportSchedules));
            if (enumValues.Length > 0)
            {
                foreach (int item in enumValues)
                {
                    dataColumn = new DataColumn(Enum.GetName(typeof(ExportSchedules), item));
                    dt.Columns.Add(dataColumn);
                }
            }
            //添加内容
            foreach (var row in list)
            {
                dataRow = dt.NewRow();
                dataRow[ExportSchedules.所属专业.ToString()] = row.Profession?.Name;
                dataRow[ExportSchedules.施工部位.ToString()] = row.Location;
                dataRow[ExportSchedules.任务名称.ToString()] = row.Name;
                dataRow[ExportSchedules.创建时间.ToString()] = row.CreationTime;
                dataRow[ExportSchedules.开始时间.ToString()] = row.StartTime;
                dataRow[ExportSchedules.完成时间.ToString()] = row.EndTime;
                dataRow[ExportSchedules.施工工期.ToString()] = row.TimeLimit;
                dataRow[ExportSchedules.当前进度.ToString()] = row.Progress + "%";
                dataRow[ExportSchedules.审核方式.ToString()] = row.ScheduleFlowInfos == null ? "人工审核" : "自动审核";
                dataRow[ExportSchedules.任务状态.ToString()] = row.State.GetDescription();
                dt.Rows.Add(dataRow);
            }
            sbuf = ExcelHelper.DataTableToExcel(dt, "任务计划表.xlsx");
            stream = new MemoryStream(sbuf);
            return Task.FromResult(stream);
        }

        public async Task<List<SingleFlowNodeDto>> GetFlowInfo(Guid workFlowId, Guid scheduleId)
        {
            return await GetRunFlowInfo(workFlowId, scheduleId);
        }

        #region 私有方法

        private async void RecursionAdd(List<ProcessTemplateDto> processTemplates, Schedule parentSchedule, ScheduleCreateDto input, ScheduleFlowTemplate scheduleFlow,List<Schedule> scheduleList)
        {
            foreach (var processTemplate in processTemplates)
            {
                var schedule = new Schedule(_guidGenerator.Create())
                {
                    ProfessionId = input.ProfessionId,
                    Location = input.Location,
                    Type = input.Type,
                    IsAuto = input.IsAuto,
                    State = State.NoStart,

                    ParentId = parentSchedule?.Id,
                    Name = processTemplate.Name,
                    TimeLimit = (double)(processTemplate.DurationUnit == StdBasic.Enums.ServiceLifeUnit.YEAR ? processTemplate.Duration * 365 : (processTemplate.DurationUnit == StdBasic.Enums.ServiceLifeUnit.MONTH ? processTemplate.Duration * 30 : processTemplate.Duration)),
                    //ProjectType = input.ProjectType, 工程类型改为关联表
                };
                //保存关联前置任务 -- 批量添加时，工序模板只能选一个前置任务
                if (processTemplate.PrepositionId != null)
                {
                    schedule.ScheduleRltSchedules = new List<ScheduleRltSchedule>();
                    schedule.ScheduleRltSchedules.Add(new ScheduleRltSchedule(_guidGenerator.Create())
                    {
                        ScheduleId = schedule.Id,
                        FrontScheduleId = (Guid)processTemplate.PrepositionId,
                    });
                }
                //保存关联工程工项
                schedule.ScheduleRltProjectItems = new List<ScheduleRltProjectItem>();
                var rltProjectItem = _projectItemRltProcessTemplatesRepository.Where(x => x.ProcessTemplateId == processTemplate.Id).ToList();
                foreach (var projectItem in rltProjectItem)
                {
                    schedule.ScheduleRltProjectItems.Add(new ScheduleRltProjectItem(_guidGenerator.Create())
                    {
                        ScheduleId = schedule.Id,
                        ProjectItemId = projectItem.ProjectItemId,
                    });
                }

                await _scheduleRepository.InsertAsync(schedule);
                scheduleList.Add(schedule);

                //ApprovalSchedule(schedule, input.WorkflowTemplateId, input.IsAuto);  //将审批通过

                if (processTemplate.Children.Count > 0)
                {
                    RecursionAdd(processTemplate.Children, schedule, input, scheduleFlow, scheduleList);
                }
            }
        }

        private async void ApprovalSchedule(Schedule schedule,Guid? workflowTemplateId, bool isAuto)
        {
            var oldSchedule = _scheduleRepository.FirstOrDefault(x => x.Id == schedule.Id);
            Workflow workflow;

            if (workflowTemplateId != null && workflowTemplateId != Guid.Empty) //当为人工审核时
            {
                // 先创建一个并启动一个计划
                workflow = await _singleFlowProcess.CreateSingleWorkFlow((Guid)workflowTemplateId);
                oldSchedule.WorkflowTemplateId = workflowTemplateId;
                oldSchedule.WorkflowId = workflow.Id;
            }
            else //当为自动审核，则找到已设置的模板，去赋值
            {
                var scheduleflow = _scheduleFlowTemplateRepository.FirstOrDefault();
                if (scheduleflow != null)
                {
                    oldSchedule.WorkflowTemplateId = scheduleflow.WorkflowTemplateId;
                    // 先创建一个并启动一个计划
                    workflow = await _singleFlowProcess.CreateSingleWorkFlow(scheduleflow.WorkflowTemplateId);
                    oldSchedule.WorkflowId = workflow.Id;
                }
                else
                {
                    throw new UserFriendlyException("请先设置自动审核流程！");
                }
            }

            if (isAuto) //如果是自动审核，则自动保存审核信息，且计划审批变为已完成
            {
                //获取计划关联流程的各个节点的名称及审批人员信息
                var flowNodes = await GetSingleFlowNodes((Guid)oldSchedule.WorkflowId, schedule.Id);

                //递归将每个节点审批通过
                await EverApprovalAsync(flowNodes, oldSchedule.Id);
                await CurrentUnitOfWork.SaveChangesAsync();

                oldSchedule.State = State.Finshed;
            }


            await _scheduleRepository.UpdateAsync(oldSchedule);
            await CurrentUnitOfWork.SaveChangesAsync();

            //return await Task.FromResult(ObjectMapper.Map<Schedule, ScheduleDto>(oldSchedule));
        }


        private async Task<bool> CheckSameName(Guid? parentId, Guid? id, string name)
        {
            return await Task.Run(() =>
            {
                var sameNames =
                    _scheduleRepository.FirstOrDefault(a =>
                        a.Name == name && a.ParentId == parentId && a.Id != id);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前级别下已存在该名称的任务！");
                }

                return true;
            });
        }

        public async Task<List<SingleFlowNodeDto>> GetSingleFlowNodes(Guid workflowId, Guid scheduleId)
        {
            var nodes = await _singleFlowProcess.GetWorkFlowNodes(workflowId);
            var infos = await _scheduleFlowInfoRepository.Where(a => a.ScheduleId == scheduleId).ToListAsync();
            foreach (var node in nodes)
            {
                node.Comments ??= new List<CommentDto>();
                node.Approvers?.ForEach(a =>
                {
                    var info = infos.FirstOrDefault(b => b.CreatorId == a.Id);
                    var comment = new CommentDto()
                    {
                        User = a,
                        Content = info.Content,
                        ApproveTime = info?.CreationTime ?? default
                    };
                    node.Comments.Add(comment);
                });
            }
            return await _singleFlowProcess.GetNodeTree(nodes);
        }

        private async Task<List<SingleFlowNodeDto>> GetRunFlowInfo(Guid workflowId, Guid scheduleId)
        {
            SingleFlowNodeDto endNode;
            DateTime? endTime = null;
            var nodes = await _singleFlowProcess.GetWorkFlowNodes(workflowId);
            var schedule = _scheduleRepository.FirstOrDefault(x => x.Id == scheduleId);
            //获取workflowData中的节点审批状态及审批意见
            var infos = _workflowDataRepository.Where(x => x.WorkflowId == workflowId).ToList();

            WorkflowData info;
            foreach (var node in nodes)
            {
                if (node.Type == "bpmStart")
                {
                    info = infos.FirstOrDefault(b => b.TargetNodeId == null || b.TargetNodeId == Guid.Empty);
                }
                else
                {
                    info = infos.FirstOrDefault(b => b.TargetNodeId == node.Id); //获取 除开始节点外，其余节点对应的信息
                }
                //获取结束时间：
                var workFlow = _workflowsRepository.FirstOrDefault(x => x.Id == workflowId);
                if (node.Type == "bpmEnd" && workFlow?.State == Bpm.WorkflowState.Finished)
                {
                    endNode = nodes.FirstOrDefault(x => x.Type == "bpmEnd");
                    var data = infos.FirstOrDefault(x => x.TargetNodeId == endNode.ParentId);  //前提是结束节点前只能有一个抄送节点指向他，否则，结束时间不准确
                    if (data != null)
                    {
                        node.Comments ??= new List<CommentDto>();
                        endTime = data.CreationTime;
                        var comment = new CommentDto()
                        {
                            ApproveTime = (DateTime)(endTime),
                        };
                        node.Comments.Add(comment);
                    }
                }
                node.Comments ??= new List<CommentDto>();
                node.Approvers?.ForEach(a =>
                {
                    var comment = new CommentDto()
                    {
                        User = a,
                        ApproveTime = (DateTime)(endTime != null ? endTime : info?.CreationTime ?? default),
                    };
                    node.Comments.Add(comment);
                });
            }
            return await _singleFlowProcess.GetNodeTree(nodes);
        }

        private async Task<bool> Approve(Guid id, Guid? userId = null)
        {
            var plan = _scheduleRepository.FirstOrDefault(a => a.Id == id);
            var detial = plan.WorkflowId.GetValueOrDefault();
            var result = await _singleFlowProcess.Approved(detial, null,userId);// 关键代码
            await CurrentUnitOfWork.SaveChangesAsync(); //不加会导致第二个审批节点通过不了
            var info = new ScheduleFlowInfo(_guidGenerator.Create())
            {
                Content = "系统自动审核",
                ScheduleId = id,
                WorkFlowId = result.Id,
                State = result.State,
            };
            await _scheduleFlowInfoRepository.InsertAsync(info);

            return true;
        }

        private async Task EverApprovalAsync(List<SingleFlowNodeDto> flowNodes, Guid scheduleId)
        {
            foreach (var flowNode in flowNodes)
            {

                if (flowNode.Approvers.Count > 0)
                {
                    foreach (var approval in flowNode.Approvers)
                    {
                        await Approve(scheduleId, approval.Id);
                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                }


                if (flowNode.Children != null && flowNode.Children.Count > 0)
                {
                    await EverApprovalAsync(flowNode.Children, scheduleId);
                }
            }
        }
        #endregion
    }
}
