using Newtonsoft.Json.Linq;
using SnAbp.Emerg.Dtos;
using SnAbp.Emerg.Entities;
using SnAbp.Emerg.Enums;
using SnAbp.Emerg.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using SnAbp.Identity;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Emerg.Authorization;
using Volo.Abp.Data;

namespace SnAbp.Emerg.Services
{
    [Authorize]
    public class EmergFaultAppService : EmergAppService, IEmergFaultAppService
    {
        DateTime dt = DateTime.Now;
        private readonly IRepository<Fault, Guid> _faultRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<FaultRltComponentCategory, Guid> _faultRltComponentCategoriesRepository;
        private readonly IRepository<FaultRltEquipment, Guid> _faultRltEquipmentRepository;
        private readonly IRepository<EmergPlanProcessRecord> _emergPlanProcessRecord;
        private readonly IRepository<EmergPlan, Guid> _emergPlanRepository;
        private readonly IRepository<EmergPlanRecord, Guid> _emergPlanRecordsRepository;
        private readonly IRepository<EmergPlanRltComponentCategory, Guid> _emergPlanRltComponentCategory;
        private readonly IRepository<EmergPlanRltFile, Guid> _emergPlanRltFile;
        private readonly IRepository<EmergPlanRecordRltMember, Guid> _emergPlanRecordRltMember;
        private readonly IdentityUserManager _identityUserManager;   //获取当前用户仓储
        private readonly IHttpContextAccessor _httpContextAccessor;  //获取当前用户登录的组织机构仓储
        private readonly IIdentityUserRepository _userRepository;
        private readonly IDataFilter _dataFilter;
        public EmergFaultAppService(
            IRepository<Fault, Guid> faultRepository,
            IRepository<FaultRltComponentCategory, Guid> faultRltComponentCategoriesRepository,
            IRepository<FaultRltEquipment, Guid> faultRltEquipmentRepository,
            IRepository<EmergPlan, Guid> emergPlanRepository,
            IRepository<EmergPlanRecord, Guid> emergPlanRecordsRepository,
            IRepository<EmergPlanRltComponentCategory, Guid> emergPlanRltComponentCategory,
            IRepository<EmergPlanRltFile, Guid> emergPlanRltFile,
            IRepository<EmergPlanRecordRltMember, Guid> emergPlanRecordRltMember,
            IGuidGenerator guidGenerator,
            IRepository<EmergPlanRecordRltMember> emergPlanRecordRltMembersRepository,
            IdentityUserManager identityUserManager,
            IHttpContextAccessor httpContextAccessor,
            IRepository<EmergPlanProcessRecord> emergPlanProcessRecord,
            IIdentityUserRepository userRepository,
            IDataFilter dataFilter
            )
        {
            _faultRepository = faultRepository;
            _faultRltComponentCategoriesRepository = faultRltComponentCategoriesRepository;
            _faultRltEquipmentRepository = faultRltEquipmentRepository;
            _emergPlanRepository = emergPlanRepository;
            _emergPlanRecordsRepository = emergPlanRecordsRepository;
            _emergPlanRltComponentCategory = emergPlanRltComponentCategory;
            _emergPlanRltFile = emergPlanRltFile;
            _emergPlanRecordRltMember = emergPlanRecordRltMember;
            _guidGenerator = guidGenerator;
            _identityUserManager = identityUserManager;
            _httpContextAccessor = httpContextAccessor;
            _emergPlanProcessRecord = emergPlanProcessRecord;
            _userRepository = userRepository;
            _dataFilter = dataFilter;
        }

        [Authorize(EmergPermissions.Fault.Create)]
        public async Task<FaultDto> Create(FaultCreateDto input)
        {
            if (input.OrganizationId == null) throw new UserFriendlyException("请选择车间工区");
            if (input.RailwayId == null) throw new UserFriendlyException("请选择所属线别");
            if (input.StationId == null) throw new UserFriendlyException("请选择车站区间");
            if (input.FaultRltComponentCategories == null || input.FaultRltComponentCategories.Count < 0) throw new UserFriendlyException("请选择设备类型");
            if (input.EquipmentNames == null) throw new UserFriendlyException("请输入车站区间");
            if (input.CheckInTime == null) throw new UserFriendlyException("请选择故障时间");
            if (input.Content == null) throw new UserFriendlyException("请输入故障现象");
            if (input.ReasonTypeId == null) throw new UserFriendlyException("请选择原因分类");
            if (input.LevelId == null) throw new UserFriendlyException("请选择原因分类");
            if (input.CheckInTime > dt) throw new UserFriendlyException("故障时间不能超过当前时间！");
            if (input.CheckOutTime != null)
            {
                if (input.CheckOutTime > dt || input.CheckOutTime <= input.CheckInTime)
                {
                    throw new UserFriendlyException("销记时间超过了当前时间或在故障时间之前！");
                }
            }

            var FaultId = _guidGenerator.Create();
            var fault = new Fault(FaultId);

            fault.OrganizationId = input.OrganizationId;
            fault.RailwayId = input.RailwayId;
            fault.StationId = input.StationId;
            fault.EquipmentNames = input.EquipmentNames;
            fault.CheckInTime = input.CheckInTime;
            fault.CheckOutTime = input.CheckOutTime;
            fault.Content = input.Content;
            fault.ReasonTypeId = input.ReasonTypeId;
            fault.Abnormal = input.Abnormal;
            fault.LevelId = input.LevelId;
            fault.WeatherDetail = input.WeatherDetail;
            fault.TemperatureMax = input.TemperatureMax;
            fault.TemperatureMin = input.TemperatureMin;
            fault.Summary = input.Summary;
            fault.DisposeProcess = input.DisposeProcess;
            fault.Reason = input.Reason;
            fault.DisposePersons = input.DisposePersons;
            fault.Remark = input.Remark;
            fault.State = input.State;

            //重新保存关联构件表信息
            fault.FaultRltComponentCategories = new List<FaultRltComponentCategory>();
            foreach (var componentCategories in input.FaultRltComponentCategories)
            {
                fault.FaultRltComponentCategories.Add(new FaultRltComponentCategory(_guidGenerator.Create())
                {
                    ComponentCategoryId = componentCategories.Id
                });
            }

            //重新保存关联设备表信息
            fault.FaultRltEquipments = new List<FaultRltEquipment>();
            foreach (var equiments in input.FaultRltEquipments)
            {
                fault.FaultRltEquipments.Add(new FaultRltEquipment(_guidGenerator.Create())
                {
                    EquipmentId = equiments.Id
                });
            }

            await _faultRepository.InsertAsync(fault);
            return ObjectMapper.Map<Fault, FaultDto>(fault);
        }
        [Authorize(EmergPermissions.Fault.Detail)]
        public async Task<FaultDto> Get(Guid id)
        {
            if (id == Guid.Empty || id == null) throw new UserFriendlyException("Id不能为空");
            var fault = _faultRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            var equipmentGroupNames = _faultRltEquipmentRepository.WithDetails(x => x.FaultId == id);
            if (fault == null) throw new UserFriendlyException("该故障不存在");

            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);

            var memberIds = members.Select(x => x.Id).ToList();

            var faultInfo = ObjectMapper.Map<Fault, FaultDto>(fault);

            if (fault.EmergPlanRecord != null)
            {
                //查询当处理人名称
                var userIds = fault.EmergPlanRecord?.ProcessRecords.Select(x => x.UserId).ToList();

                var users = _userRepository.Where(x => userIds.Contains(x.Id)).ToList();

                faultInfo.EmergPlanRecord.ProcessRecords?.ForEach(processRecord =>
                {
                    var target = users.Find(x => x.Id == processRecord.UserId);
                    processRecord.HandleUserName = target?.UserName;
                });

                fault.EmergPlanRecord.ProcessRecords = fault.EmergPlanRecord.ProcessRecords.OrderBy(x => x.Time).ToList();

                //判断当前用户是否有处理权限
                JToken flowData = JToken.Parse(fault.EmergPlanRecord.Flow);

                var nodes = JsonConvert.DeserializeObject<List<EmergPlanFlowNode>>(
                    flowData["nodes"].ToString(),
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                nodes?.ForEach(node =>
                {
                    if (node.Active)
                    {
                        node.Members?.ForEach(member =>
                        {
                            if (memberIds.Contains(member.Id))
                            {
                                faultInfo.CurrentNodeIds.Add(node.Id);
                            }
                        });
                    }

                });

            }
            return await Task.FromResult(faultInfo);
        }

        public async Task<PagedResultDto<FaultSimpleDto>> GetList(FaultSearchDto input)
        {
            var members = await _identityUserManager.GetUserMembers(CurrentUser.Id.Value);
            var memberIds = members.Select(x => x.Id).ToList();
            var memberUser = members.Where(x => x.Type == MemberType.User);

            var organizationId = _httpContextAccessor.HttpContext.Request.Headers["OrganizationId"];
            var fault = _faultRepository.WithDetails()
                //.WhereIf(memberIds != null && memberIds.Count > 0, x => x.EmergPlanRecord.EmergPlanRecordRltMembers.Any(y => memberIds.Contains(y.MemeberId)))
                .WhereIf(input.PendingAndUnchecked == 1000, x => x.State == FaultState.Pending || x.State == FaultState.UnChecked)
                .WhereIf(!string.IsNullOrEmpty(organizationId) && organizationId.First() != Guid.Empty.ToString(), x => x.OrganizationId.ToString() == organizationId.First())
                .WhereIf(input.OrganizationId != null && input.OrganizationId != Guid.Empty, x => x.OrganizationId == input.OrganizationId)
                .WhereIf(input.RailwayId != null && input.RailwayId != Guid.Empty, x => x.RailwayId == input.RailwayId)
                .WhereIf(input.StationId != null && input.StationId != Guid.Empty, x => x.StationId == input.StationId)
                .WhereIf(input.ComponentCategoryIds != null && input.ComponentCategoryIds.Count > 0, x => x.FaultRltComponentCategories.Any(y => input.ComponentCategoryIds.Contains(y.ComponentCategoryId)))
                .WhereIf(input.EquipmentIds != null && input.EquipmentIds.Count > 0, x => x.FaultRltEquipments.Any(y => input.EquipmentIds.Contains(y.EquipmentId)))
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Content.Contains(input.Keywords) || x.Abnormal.Contains(input.Keywords) 
                || x.Reason.Contains(input.Keywords) || x.DisposeProcess.Contains(input.Keywords) || x.Remark.Contains(input.Keywords))
                .WhereIf(input.StartTime != null && input.EndTime != null, x => DateTime.Compare(x.CheckInTime, (DateTime)input.StartTime) >= 0 && DateTime.Compare(x.CheckInTime, (DateTime)input.EndTime) <= 0)
                .WhereIf(input.State.IsIn(FaultState.Submitted, FaultState.UnChecked, FaultState.CheckedOut, FaultState.UnSubmitted, FaultState.Pending), x => x.State == input.State)
                .WhereIf(input.Group.IsIn(EmergPlanRecordRltMemberGroup.Waiting, EmergPlanRecordRltMemberGroup.Processed, EmergPlanRecordRltMemberGroup.Cc),
                x => x.EmergPlanRecord.EmergPlanRecordRltMembers.Any(a => a.Group == input.Group) && x.EmergPlanRecord.EmergPlanRecordRltMembers.Any(y => memberIds.Contains(y.MemeberId))) //判断是否为待我处理、我已处理、抄送给我
                .WhereIf(input.Group == EmergPlanRecordRltMemberGroup.Launched, x => x.CreatorId == memberUser.Select(m => m.Id).First());    //判断是否为我发起的
            var result = new PagedResultDto<FaultSimpleDto>();
            result.TotalCount = fault.Count();
            result.Items = ObjectMapper.Map<List<Fault>, List<FaultSimpleDto>>(fault.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return await Task.FromResult(result);
        }

        [Authorize(EmergPermissions.Fault.Update)]
        public async Task<FaultDto> Update(FaultUpdateDto input)
        {
            var fault = await _faultRepository.GetAsync(input.Id);
            if (fault == null) throw new UserFriendlyException("该故障不存在");
            if (input.EmergPlanRecordId == null)
            {
                if (input.OrganizationId == null) throw new UserFriendlyException("请选择车间工区");
                if (input.RailwayId == null) throw new UserFriendlyException("请选择所属线别");
                if (input.StationId == null) throw new UserFriendlyException("请选择车站区间");
                if (input.FaultRltComponentCategories == null || input.FaultRltComponentCategories.Count < 0) throw new UserFriendlyException("请选择设备类型");
                if (input.EquipmentNames == null) throw new UserFriendlyException("请选择车站区间");
                if (input.CheckInTime == null) throw new UserFriendlyException("请选择故障时间");
                if (input.Content == null) throw new UserFriendlyException("请输入故障现象");
                if (input.ReasonTypeId == null) throw new UserFriendlyException("请选择原因分类");
                if (input.CheckInTime > dt) throw new UserFriendlyException("故障时间不能超过当前时间！");
                if (input.CheckOutTime != null && (input.CheckOutTime > dt || input.CheckOutTime <= input.CheckInTime)) throw new UserFriendlyException("销记时间超过了当前时间或在故障时间之前！");



                //清除之前关联构件表信息
                await _faultRltComponentCategoriesRepository.DeleteAsync(input.Id);
                //重新保存关联构件表信息
                fault.FaultRltComponentCategories = new List<FaultRltComponentCategory>();
                foreach (var componentCategories in input.FaultRltComponentCategories)
                {
                    fault.FaultRltComponentCategories.Add(new FaultRltComponentCategory(_guidGenerator.Create())
                    {
                        ComponentCategoryId = componentCategories.Id
                    });
                }
                //清除之前关联设备表信息
                await _faultRltEquipmentRepository.DeleteAsync(input.Id);
                //重新保存关联设备表信息
                fault.FaultRltEquipments = new List<FaultRltEquipment>();
                foreach (var equiments in input.FaultRltEquipments)
                {
                    fault.FaultRltEquipments.Add(new FaultRltEquipment(_guidGenerator.Create())
                    {
                        EquipmentId = equiments.Id
                    });
                }

                fault.OrganizationId = input.OrganizationId;
                fault.RailwayId = input.RailwayId;
                fault.StationId = input.StationId;
                fault.EquipmentNames = input.EquipmentNames;
                fault.CheckInTime = input.CheckInTime;
                fault.Content = input.Content;
                fault.ReasonTypeId = input.ReasonTypeId;
                fault.Abnormal = input.Abnormal;
                fault.LevelId = input.LevelId;
                fault.WeatherDetail = input.WeatherDetail;
                fault.TemperatureMax = input.TemperatureMax;
                fault.TemperatureMin = input.TemperatureMin;
                fault.Summary = input.Summary;
                fault.DisposeProcess = input.DisposeProcess;
                fault.Reason = input.Reason;
                fault.DisposePersons = input.DisposePersons;
                fault.Remark = input.Remark;
                fault.Source = input.Source;
                fault.State = input.State;
                fault.CheckOutTime = input.CheckOutTime;
                fault.CheckInUserId = input.CheckInUserId;
                fault.CheckOutUserId = input.CheckOutUserId;
                fault.SubmitUserId = input.SubmitUserId;

                await _faultRepository.UpdateAsync(fault);
                return ObjectMapper.Map<Fault, FaultDto>(fault);
            }
            else
            {
                fault.EmergPlanRecordId = input.EmergPlanRecordId;
                await _faultRepository.UpdateAsync(fault);
                return ObjectMapper.Map<Fault, FaultDto>(fault);

            }
        }

        [Authorize(EmergPermissions.Fault.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            await _faultRepository.DeleteAsync(id);
            return true;
        }

        [Authorize(EmergPermissions.Plan.Apply)]
        public async Task<EmergPlanRecordDto> ApplyEmergPlan(ApplyEmergPlanDto input)
        {
            //判断 input.id 的故障是否存在
            if (input.FaultId == null || Guid.Empty == input.FaultId) throw new UserFriendlyException("Fault is null");
            var fault = await _faultRepository.GetAsync(input.FaultId);
            if (fault == null) throw new UserFriendlyException("该故障不存在");

            // 通过调用的Id查询预案
            if (input.EmergPlanId == null || Guid.Empty == input.EmergPlanId) throw new UserFriendlyException("Fault is null");
            var emergPlan = _emergPlanRepository.WithDetails().Where(x => x.Id == input.EmergPlanId).FirstOrDefault();
            if (emergPlan == null) throw new UserFriendlyException("该预案不存在");
            // 创建 emrgPlanRecord 
            var emergPlanRecordId = _guidGenerator.Create();
            var emergPlanRecord = new EmergPlanRecord(emergPlanRecordId);
            //更新故障记录表id
            fault.EmergPlanRecordId = emergPlanRecordId;

            //初始化开始节点及激活状态
            JToken flowData = JToken.Parse(emergPlan.Flow);
            var nodes = JsonConvert.DeserializeObject<List<EmergPlanFlowNode>>(
                                flowData["nodes"].ToString(),
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            EmergPlanFlowNode startNode = nodes.Find(x => x.Type == "bpmStart");
            startNode.Active = true;
            EmergPlanFlowNode otherNode = nodes.Find(x => x.Type != "bpmStart");
            otherNode.Active = false;
            flowData["nodes"] = JToken.Parse(JsonConvert.SerializeObject(nodes));
            //添加记录表
            fault.State = FaultState.UnChecked;

            emergPlanRecord.Name = emergPlan.Name; //名称
            emergPlanRecord.LevelId = emergPlan.LevelId;//预案等级
            emergPlanRecord.Summary = emergPlan.Summary;//预案摘要
            emergPlanRecord.Remark = emergPlan.Remark; //备注
            emergPlanRecord.Flow = JsonConvert.SerializeObject(flowData);//预案流程
            emergPlanRecord.Content = emergPlan.Content;//预案图文资料

            //保存预案-构件关联表
            List<EmergPlanRltComponentCategory> componentCategory = _emergPlanRltComponentCategory.WithDetails().Where(x => x.EmergPlanId == input.EmergPlanId).ToList();
            List<Guid> componentCategoryIds = new List<Guid>();
            foreach (var item in componentCategory)
            {
                componentCategoryIds.Add(item.ComponentCategoryId);
            }


            emergPlanRecord.EmergPlanRecordRltComponentCategories = new List<EmergPlanRecordRltComponentCategory>();

            foreach (var componentCategoryId in componentCategoryIds)
            {
                emergPlanRecord.EmergPlanRecordRltComponentCategories.Add(new EmergPlanRecordRltComponentCategory(_guidGenerator.Create())
                {
                    ComponentCategoryId = componentCategoryId
                });
            }

            //保存预案-文件关联表
            var file = _emergPlanRltFile.WithDetails().Where(x => x.EmergPlanId == input.EmergPlanId).ToList();

            List<Guid> fileIds = new List<Guid>();

            foreach (var item in file)
            {
                fileIds.Add(item.FileId);
            }

            emergPlanRecord.EmergPlanRecordRltFiles = new List<EmergPlanRecordRltFile>();

            foreach (var fileId in fileIds)
            {
                emergPlanRecord.EmergPlanRecordRltFiles.Add(new EmergPlanRecordRltFile(_guidGenerator.Create())
                {
                    FileId = fileId
                });
            }

            await CheckMember(JsonConvert.SerializeObject(flowData), emergPlanRecordId);
            await _emergPlanRecordsRepository.InsertAsync(emergPlanRecord);
            await _faultRepository.UpdateAsync(fault);

            return ObjectMapper.Map<EmergPlanRecord, EmergPlanRecordDto>(emergPlanRecord);
        }

        //计算成员状态
        async public Task<FaultDto> Process(FaultProcessDto input)
        {
            //查询故障
            var fault = _faultRepository.FirstOrDefault(x => x.Id == input.Id);
            var emergPlanRecord = await _emergPlanRecordsRepository.GetAsync((Guid)fault.EmergPlanRecordId);

            // 转字符串为 Json
            JToken flowData = JToken.Parse(emergPlanRecord.Flow);

            // 数据校验
            var nodes = JsonConvert.DeserializeObject<List<EmergPlanFlowNode>>(
                                flowData["nodes"].ToString(),
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var edges = JsonConvert.DeserializeObject<List<EmergPlanFlowEdge>>(
                         flowData["edges"].ToString(),
                         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            EmergPlanFlowNode targetNode = nodes.Find(x => x.Id == input.NodeId);
            targetNode.Processed = true;
            targetNode.Active = false;
            targetNode.ProcessTime = input.ProcessTime;
            await _emergPlanProcessRecord.InsertAsync(InsertPlanProcessRecord(input, fault, Guid.Empty));
            List<EmergPlanFlowNode> nextNodes = GetFlowNextNodes(nodes, edges, targetNode);//查找当前节点的下节点集合
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Type == "bpmCc")
                {
                    nextNode.Processed = true;
                }
                else if (nextNode.Type == "bpmEnd")
                {
                    fault.State = FaultState.CheckedOut;
                    fault.CheckOutTime = dt;
                    nextNode.Processed = true;
                    nextNode.ProcessTime = input.ProcessTime;
                    var planProcessRecord = InsertPlanProcessRecord(null, fault, nextNode.Id);
                    planProcessRecord.Time = input.ProcessTime.AddSeconds(1);
                    await _emergPlanProcessRecord.InsertAsync(planProcessRecord);
                }
                else if (nextNode.Type == "determine")
                {
                    EmergPlanFlowNode determineNextNode = nodes.Find(x => x.Id == input.DetermineTargetId);
                    if (determineNextNode.Type == "bpmEnd")
                    {
                        determineNextNode.Processed = true;
                        determineNextNode.ProcessTime = input.ProcessTime;
                        fault.CheckOutTime = dt;
                        fault.State = FaultState.CheckedOut;
                        var planProcessRecord = InsertPlanProcessRecord(null, fault, determineNextNode.Id);
                        planProcessRecord.Time = input.ProcessTime.AddSeconds(1);
                        await _emergPlanProcessRecord.InsertAsync(planProcessRecord);
                    }
                    else
                    {
                        determineNextNode.Active = true;
                        nextNode.Processed = true;
                    }

                }
                else
                {
                    nextNode.Active = true;

                }
            }
            flowData["nodes"] = JToken.Parse(JsonConvert.SerializeObject(nodes));

            //成员关联表更新
            //删除关联表信息
            await _emergPlanRecordRltMember.DeleteAsync(x => x.EmergPlanRecordId == fault.EmergPlanRecordId);
            foreach (var node in nodes)
            {
                if (node.Active && node.Type != "bpmCc")
                {
                    if (node.Members == null)
                    {
                        continue;
                    }
                    else if (node.Members.Count > 0 && node.Members != null)
                    {
                        foreach (var member in node.Members)
                        {
                            var emergPlanRecordMemberId = _guidGenerator.Create();
                            var emergPlanRecordMember = new EmergPlanRecordRltMember(emergPlanRecordMemberId);
                            emergPlanRecordMember.EmergPlanRecordId = (Guid)fault.EmergPlanRecordId;
                            emergPlanRecordMember.MemberType = member.Type;
                            emergPlanRecordMember.Group = EmergPlanRecordRltMemberGroup.Waiting;
                            emergPlanRecordMember.MemeberId = member.Id;
                            await _emergPlanRecordRltMember.InsertAsync(emergPlanRecordMember);
                        }

                    }

                }
                else if (node.Processed && node.Type != "bpmCc")
                {
                    if (node.Members == null)
                    {
                        continue;
                    }
                    if (node.Members.Count > 0 && node.Members != null)
                    {
                        foreach (var member in node.Members)
                        {
                            var emergPlanRecordMemberId = _guidGenerator.Create();
                            var emergPlanRecordMember = new EmergPlanRecordRltMember(emergPlanRecordMemberId);
                            emergPlanRecordMember.EmergPlanRecordId = (Guid)fault.EmergPlanRecordId;
                            emergPlanRecordMember.MemberType = member.Type;
                            emergPlanRecordMember.Group = EmergPlanRecordRltMemberGroup.Processed;
                            emergPlanRecordMember.MemeberId = member.Id;
                            await _emergPlanRecordRltMember.InsertAsync(emergPlanRecordMember);
                        }
                    }
                }
                else if (node.Type == "bpmCc" && node.Processed)
                {
                    if (node.Members == null)
                    {
                        continue;
                    }
                    if (node.Members.Count > 0 && node.Members != null)
                    {
                        foreach (var member in node.Members)
                        {
                            var emergPlanRecordMemberId = _guidGenerator.Create();
                            var emergPlanRecordMember = new EmergPlanRecordRltMember(emergPlanRecordMemberId);
                            emergPlanRecordMember.EmergPlanRecordId = (Guid)fault.EmergPlanRecordId;
                            emergPlanRecordMember.MemberType = member.Type;
                            emergPlanRecordMember.Group = EmergPlanRecordRltMemberGroup.Cc;
                            emergPlanRecordMember.MemeberId = member.Id;
                            await _emergPlanRecordRltMember.InsertAsync(emergPlanRecordMember);
                        }
                    }
                }
            }

            //更新调用记录表flow
            emergPlanRecord.Flow = JsonConvert.SerializeObject(flowData);
            await _emergPlanRecordsRepository.UpdateAsync(emergPlanRecord);
            await _faultRepository.UpdateAsync(fault);
            return ObjectMapper.Map<Fault, FaultDto>(fault);
        }

        //私有方法,初始化成员表
        private Task<bool> CheckMember(string flow, Guid emergPlanRecordId)
        {
            //判断是否存在Json数据
            if (string.IsNullOrEmpty(flow))
            {
                throw new UserFriendlyException("flow is null");
            }
            else
            {
                JToken flowData = JToken.Parse(flow);

                var nodes = JsonConvert.DeserializeObject<List<EmergPlanFlowNode>>(
                                flowData["nodes"].ToString(),
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                var edges = JsonConvert.DeserializeObject<List<EmergPlanFlowEdge>>(
                             flowData["edges"].ToString(),
                             new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });


                //计算变化节点属性
                foreach (var node in nodes)
                {
                    if (node.Active  && node.Processed == false)
                    {
                        if (node.Members.Count > 0 && node.Members != null)
                        {
                            foreach (var member in node.Members)
                            {
                                var emergPlanRecordMemberId = _guidGenerator.Create();
                                var emergPlanRecordMember = new EmergPlanRecordRltMember(emergPlanRecordMemberId);
                                emergPlanRecordMember.EmergPlanRecordId = emergPlanRecordId;
                                emergPlanRecordMember.MemberType = member.Type;
                                emergPlanRecordMember.Group = EmergPlanRecordRltMemberGroup.Waiting;
                                emergPlanRecordMember.MemeberId = member.Id;
                                _emergPlanRecordRltMember.InsertAsync(emergPlanRecordMember);
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("开始节点成员为空");
                        }
                    }
                }
            }

            return Task.FromResult(true);
        }
        //私有方法 获取指定节点下一步节点的集合
        private List<EmergPlanFlowNode> GetFlowNextNodes(List<EmergPlanFlowNode> nodes, List<EmergPlanFlowEdge> edges, EmergPlanFlowNode target)
        {
            var nextNodeIds = new List<Guid>();

            foreach (var edge in edges)
            {
                if (edge.Source == target.Id)
                {
                    nextNodeIds.Add(edge.Target);
                }
            }

            return nodes.Where(x => nextNodeIds.Contains(x.Id)).ToList();
        }

        //插入过程记录表
        private EmergPlanProcessRecord InsertPlanProcessRecord(FaultProcessDto input, Fault fault, Guid nodeId)
        {
            var emergPlanProcessRecordId = _guidGenerator.Create();
            var emergPlanProcessRecord = new EmergPlanProcessRecord(emergPlanProcessRecordId);

            //var emergPlanProcessRecord = new EmergPlanProcessRecordDto();
            //添加步骤记录表

            if (input != null)
            {
                emergPlanProcessRecord.Comments = input.Comments;
                emergPlanProcessRecord.NodeId = input.NodeId;
                if (CurrentUser.Id != null)
                {
                    emergPlanProcessRecord.UserId = CurrentUser.Id.Value;
                }


                emergPlanProcessRecord.Time = input.ProcessTime;
                emergPlanProcessRecord.EmergPlanRecordId = fault.EmergPlanRecordId.GetValueOrDefault();
            }
            else
            {
                emergPlanProcessRecord.Comments = "预案结束";
                emergPlanProcessRecord.NodeId = nodeId;
                emergPlanProcessRecord.UserId = Guid.Empty;
                emergPlanProcessRecord.EmergPlanRecordId = fault.EmergPlanRecordId.GetValueOrDefault();
            }

            return emergPlanProcessRecord;
        }

    }
}