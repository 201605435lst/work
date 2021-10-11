using SnAbp.Bpm.Entities;
using SnAbp.Schedule.Dtos;
using SnAbp.Schedule.Entities;
using SnAbp.Schedule.Enums;
using SnAbp.Schedule.IServices;
using SnAbp.Utils.EnumHelper;
using SnAbp.Utils.ExcelHelper;
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
    public class ScheduleApprovalAppService : ScheduleAppService, IScheduleApprovalAppService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Approval, Guid> _approvalRepository;
        private readonly IRepository<ApprovalRltFile, Guid> _approvalRltFileRepository;
        private readonly IRepository<ApprovalRltMaterial, Guid> _approvalRltMaterialRepository;
        private readonly IRepository<ApprovalRltMember, Guid> _approvalRltMemberRepository;
        private readonly IRepository<Schedule,Guid> _schedulesRepository;
        private readonly IRepository<Workflow, Guid> _workflowsRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ScheduleApprovalAppService(
            IGuidGenerator guidGenerator,
            IRepository<Approval, Guid> approvalRepository,
            IRepository<ApprovalRltFile, Guid> approvalRltFileRepository,
            IRepository<ApprovalRltMaterial, Guid> approvalRltMaterialRepository,
            IRepository<ApprovalRltMember, Guid> approvalRltMemberRepository,
            IRepository<Schedule, Guid> schedulesRepository,
            IRepository<Workflow, Guid> workflowsRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _guidGenerator = guidGenerator;
            _approvalRepository = approvalRepository;
            _approvalRltFileRepository = approvalRltFileRepository;
            _approvalRltMaterialRepository = approvalRltMaterialRepository;
            _approvalRltMemberRepository = approvalRltMemberRepository;
            _schedulesRepository = schedulesRepository;
            _workflowsRepository = workflowsRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        public async Task<ApprovalDto> Create(ApprovalCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Organization)) throw new UserFriendlyException("施工单位不能为空");
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("任务名称不能为空");
            if (string.IsNullOrEmpty(input.Code)) throw new UserFriendlyException("编号不能为空");
            if (input.Time == null) throw new UserFriendlyException("施工日期不能为空");
            if (input.ScheduleId == null) throw new UserFriendlyException("施工计划不能为空");
            if (input.ProfessionId == null) throw new UserFriendlyException("所属专业不能为空");
            if (input.Location == null) throw new UserFriendlyException("施工部位不能为空");
            if (input.ApprovalRltMembers.Count < 1) throw new UserFriendlyException("负责人和施工员不能为空");
            if (input.MemberNum < 1) throw new UserFriendlyException("劳务人员不能为空");

            await CheckSameName(null, input.Name, input.Code);

            var approval = new Approval(_guidGenerator.Create())
            {
                Organization = input.Organization,
                Code = input.Code,
                Time = input.Time,
                ScheduleId = input.ScheduleId,
                ProfessionId = input.ProfessionId,
                Location = input.Location,
                Name = input.Name,
                MemberNum = input.MemberNum,

                TemporaryEquipmentId = input.TemporaryEquipmentId,
                SafetyCautionId = input.SafetyCautionId,
                Remark = input.Remark,
            };

            //保存状态
            if (input.State == StatusType.ToSubmit)
            {
                approval.State = input.State;
            }
            else
            {
                //根据计划Id，查询此条计划的审批流程状态
                var schedule = _schedulesRepository.WithDetails().FirstOrDefault(x => x.Id == input.ScheduleId);
                approval.State = schedule.Workflow.State == Bpm.WorkflowState.Waiting ? StatusType.OnReview : (schedule.Workflow.State == Bpm.WorkflowState.Finished ? StatusType.Pass : StatusType.UnPass);
            }

            //保存关联 人员信息
            approval.ApprovalRltMembers = new List<ApprovalRltMember>();
            foreach (var member in input.ApprovalRltMembers)
            {
                approval.ApprovalRltMembers.Add(new ApprovalRltMember(_guidGenerator.Create())
                {
                    ApprovalId = approval.Id,
                    MemberId = member.Id,
                    Type = member.Type
                });
            }
            approval.ApprovalRltMembers.Add(new ApprovalRltMember(_guidGenerator.Create())
            {
                ApprovalId = approval.Id,
                MemberId = CurrentUser.Id.Value,
                Type = PersonType.Initial
            });

            //保存关联 辅助信息-技术资料
            approval.ApprovalRltFiles = new List<ApprovalRltFile>();
            foreach (var file in input.ApprovalRltFiles)
            {
                approval.ApprovalRltFiles.Add(new ApprovalRltFile(_guidGenerator.Create())
                {
                    ApprovalId = approval.Id,
                    FileId = file.Id
                });
            }

            //保存关联 物资信息
            approval.ApprovalRltMaterials = new List<ApprovalRltMaterial>();
            foreach (var material in input.ApprovalRltMaterials)
            {
                approval.ApprovalRltMaterials.Add(new ApprovalRltMaterial(_guidGenerator.Create())
                {
                    ApprovalId = approval.Id,
                    MaterialName = material.MaterialName,
                    SpecModel = material.SpecModel,
                    Unit = material.Unit,
                    Number = material.Number,
                    Type = material.Type
                });
            }

            await _approvalRepository.InsertAsync(approval);
            return await Task.FromResult(ObjectMapper.Map<Approval, ApprovalDto>(approval));
        }

        public async Task<PagedResultDto<ApprovalDto>> GetList(ApprovalSearchDto input)
        {
            var canUpdate = false;
            var approval = _approvalRepository.WithDetails()
                .WhereIf(input.ProfessionId != Guid.Empty && input.ProfessionId != null, x => x.ProfessionId == input.ProfessionId)
                .WhereIf(input.StartTime != null && input.EndTime != null, x => x.Time >= input.StartTime && x.Time <= input.EndTime)
                .WhereIf(input.IsInitiate, x => x.CreatorId == CurrentUser.Id)
                //.WhereIf(input.IsCC, x => x.CreatorId == CurrentUser.Id)
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords) || x.Profession.ToString().Contains(input.Keywords));

            var unow = _unitOfWorkManager.Begin(true, false);
            foreach (var app in approval)
            {
                canUpdate = CanUpdate(app.Schedule.WorkflowId.GetValueOrDefault());
                if (canUpdate)
                {
                    app.State = StatusType.Pass;
                    await _approvalRepository.UpdateAsync(app);
                    await unow.SaveChangesAsync();
                }
            }

            var result = new PagedResultDto<ApprovalDto>
            {
                TotalCount = approval.Count(),
                Items = ObjectMapper.Map<List<Approval>, List<ApprovalDto>>(approval.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList())
            };

            return await Task.FromResult(result);
        }

        public async Task<ApprovalDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var approval = _approvalRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (approval == null) throw new UserFriendlyException("此审批不存在");
            var approvalDto = ObjectMapper.Map<Approval, ApprovalDto>(approval);
            foreach (var item in approvalDto.ApprovalRltMembers)
            {
                if (item.Type == PersonType.Manager)
                {
                    approvalDto.Directors.Add(item);
                }
                if (item.Type == PersonType.Worker)
                {
                    approvalDto.Builders.Add(item);
                }
            }
            foreach (var item in approvalDto.ApprovalRltMaterials)
            {
                if (item.Type == MaterialsType.Appliance)
                {
                    approvalDto.ApplianceList.Add(item);
                }
                if (item.Type == MaterialsType.Mechanical)
                {
                    approvalDto.MechanicalList.Add(item);
                }
                if (item.Type == MaterialsType.AutoCompute)
                {
                    approvalDto.MaterialList.Add(item);
                }
                if (item.Type == MaterialsType.SafetyArticle)
                {
                    approvalDto.SecurityProtectionList.Add(item);
                }
            }
            return approvalDto;
        }

        public async Task<string> GetCode()
        {
            var code = "";
            var nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            var approval = _approvalRepository.Where(x => x.Code.Substring(4, 8) == nowDate.Replace("-", ""));
            if (approval.Count() == 0)
            {
                code = "SGD-" + nowDate.Replace("-", "") + "001";
            }
            else
            {
                approval = approval.OrderByDescending(x => x.CreationTime); //按照时间降序后的第一个
                var maxCode = approval.FirstOrDefault().Code;
                var number = Convert.ToInt32(maxCode.Substring(12, maxCode.Length - 12)) + 1;
                if (number < 100)
                {
                    string trueNum = string.Format("{0:d3}", number);
                    code = "SGD-" + nowDate.Replace("-", "") + trueNum;
                }
                else
                {
                    code = "SGD-" + nowDate.Replace("-", "") + number;
                }
            }
            return code;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");

            await _approvalRltFileRepository.DeleteAsync(x => id == x.ApprovalId);
            await _approvalRltMaterialRepository.DeleteAsync(x => id == x.ApprovalId);
            await _approvalRltMemberRepository.DeleteAsync(x => id == x.ApprovalId);
            await _approvalRepository.DeleteAsync(x => id == x.Id);
            return true;
        }

        public Task<Stream> Export(EduceApprovalDto input)
        {
            Stream stream = null;
            if (input.IsExcel)
            {
                var approvals = _approvalRepository.WithDetails().Where(x => input.ApprovalIds.Contains(x.Id)).ToList();
                var list = approvals.OrderBy(x => x.CreationTime).ToList();
                byte[] sbuf;
                var dt = (DataTable)null;
                var dataColumn = (DataColumn)null;
                var dataRow = (DataRow)null;
                dt = new DataTable();
                //添加表头
                var enumValues = Enum.GetValues(typeof(ExportApprovals));
                if (enumValues.Length > 0)
                {
                    foreach (int item in enumValues)
                    {
                        dataColumn = new DataColumn(Enum.GetName(typeof(ExportApprovals), item));
                        dt.Columns.Add(dataColumn);
                    }
                }
                //添加内容
                foreach (var row in list)
                {
                    dataRow = dt.NewRow();
                    dataRow[ExportApprovals.所属专业.ToString()] = row.Profession?.Name;
                    dataRow[ExportApprovals.施工部位.ToString()] = row.Location;
                    dataRow[ExportApprovals.施工任务.ToString()] = row.Name;
                    dataRow[ExportApprovals.负责人.ToString()] = row.ApprovalRltMembers.Find(x => x.Type == PersonType.Manager).Member.Name;
                    dataRow[ExportApprovals.施工工期.ToString()] = row.Time;
                    dataRow[ExportApprovals.录入人.ToString()] = row.ApprovalRltMembers.Find(x => x.Type == PersonType.Initial).Member.Name;
                    dataRow[ExportApprovals.录入时间.ToString()] = row.CreationTime;
                    dataRow[ExportApprovals.审批状态.ToString()] = row.State.GetDescription();
                    dt.Rows.Add(dataRow);
                }
                sbuf = ExcelHelper.DataTableToExcel(dt, "施工审批表.xlsx");
                stream = new MemoryStream(sbuf);
            }
            
            return Task.FromResult(stream);
            throw new NotImplementedException();
        }

        public async Task<ApprovalDto> Update(ApprovalUpdateDto input)
        {
            var oldApproval = _approvalRepository.FirstOrDefault(s => s.Id == input.Id);
            if (oldApproval == null) throw new UserFriendlyException("当前更新审批不存在");

            oldApproval.Organization = input.Organization;
            oldApproval.Code = input.Code;
            oldApproval.Time = input.Time;
            oldApproval.ScheduleId = input.ScheduleId;
            oldApproval.ProfessionId = input.ProfessionId;
            oldApproval.Location = input.Location;
            oldApproval.Name = input.Name;
            oldApproval.MemberNum = input.MemberNum;

            oldApproval.TemporaryEquipmentId = input.TemporaryEquipmentId;
            oldApproval.SafetyCautionId = input.SafetyCautionId;
            oldApproval.Remark = input.Remark;
            oldApproval.State = input.State;

            //清除之前关联 信息
            await _approvalRltFileRepository.DeleteAsync(x => x.ApprovalId == input.Id);
            await _approvalRltMemberRepository.DeleteAsync(x => x.ApprovalId == input.Id);
            await _approvalRltMaterialRepository.DeleteAsync(x => x.ApprovalId == input.Id);
            // 重新保存关联 信息
            oldApproval.ApprovalRltFiles = new List<ApprovalRltFile>();
            foreach (var file in input.ApprovalRltFiles)
            {
                oldApproval.ApprovalRltFiles.Add(new ApprovalRltFile(_guidGenerator.Create())
                {
                    ApprovalId = oldApproval.Id,
                    FileId = file.Id
                });
            }

            oldApproval.ApprovalRltMaterials = new List<ApprovalRltMaterial>();
            foreach (var material in input.ApprovalRltMaterials)
            {
                oldApproval.ApprovalRltMaterials.Add(new ApprovalRltMaterial(_guidGenerator.Create())
                {
                    ApprovalId = oldApproval.Id,
                    MaterialName = material.MaterialName,
                    SpecModel = material.SpecModel,
                    Unit = material.Unit,
                    Number = material.Number,
                    Type = material.Type
                });
            }

            oldApproval.ApprovalRltMembers = new List<ApprovalRltMember>();
            foreach (var member in input.ApprovalRltMembers)
            {
                oldApproval.ApprovalRltMembers.Add(new ApprovalRltMember(_guidGenerator.Create())
                {
                    ApprovalId = oldApproval.Id,
                    MemberId = member.Id,
                    Type = member.Type
                });
            }
            oldApproval.ApprovalRltMembers.Add(new ApprovalRltMember(_guidGenerator.Create())
            {
                ApprovalId = oldApproval.Id,
                MemberId = CurrentUser.Id.Value,
                Type = PersonType.Initial
            });
            await _approvalRepository.UpdateAsync(oldApproval);
            return ObjectMapper.Map<Approval, ApprovalDto>(oldApproval);
        }


        #region 私有方法

        private async Task<bool> CheckSameName(Guid? id, string name, string code)
        {
            return await Task.Run(() =>
            {

                var sameNames =
                    _approvalRepository.FirstOrDefault(a =>
                        (a.Name == name && a.Id != id) || (a.Code == code && a.Id != id));
                if (sameNames != null && sameNames.Name == name)
                {
                    throw new UserFriendlyException("已存在该名称的任务！");
                }
                else if (sameNames != null && sameNames.Code == code)
                {
                    throw new UserFriendlyException("已存在该编号的任务！");
                }

                return true;
            });
        }

        private bool CanUpdate(Guid id)
        {
            var workFlow = _workflowsRepository.FirstOrDefault(x => x.Id == id);
            if (workFlow != null)
            {
                if (workFlow.State == Bpm.WorkflowState.Finished)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        #endregion
    }
}
