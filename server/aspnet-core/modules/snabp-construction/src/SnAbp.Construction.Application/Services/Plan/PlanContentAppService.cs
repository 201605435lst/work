using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;

using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.Dtos.Plan.Plan;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.Construction.Entities;
using SnAbp.Construction.Enums;
using SnAbp.Construction.IServices;
using SnAbp.Construction.IServices.Daily;
using SnAbp.Construction.IServices.MasterPlan;
using SnAbp.Construction.IServices.Plan;
using SnAbp.Construction.Plans;
using SnAbp.Construction.Services.MaterPlan;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.ConstructionBase.IServices;
using SnAbp.Resource.Dtos.StoreHouse;
using SnAbp.Utils.TreeHelper;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Construction.Services.Plan
{

    /// <summary>
    /// 施工计划详情 Service 
    /// </summary>
    // [Authorize]
    public class PlanContentAppService : CrudAppService<
            PlanContent, PlanContentDto, Guid, PlanContentSearchDto, PlanContentCreateDto,
            PlanContentUpdateDto>,
        IPlanContentAppService
    {
        private readonly IRepository<PlanContent, Guid> _repository;
        private readonly IRepository<Daily, Guid> _dailyRepo;
        private readonly IRepository<Plans.Plan, Guid> _planRepository;
        private readonly IRepository<PlanContentRltAntecedent, Guid> _antecedentRepository;
        private readonly IConstructionDailyAppService _dailyAppService;
        private readonly ISubItemContentAppService _subItemContentAppService;
        private readonly IMasterPlanContentAppService _masterPlanContentAppService;
        private readonly IDistributedCache<List<PlanContent>> _cachePlanContent;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDistributedCache<Plans.Plan> _currentPlan;
        public PlanContentAppService(
            IRepository<PlanContent, Guid> repository,
            IRepository<Daily, Guid> dailyRepo,
            IRepository<Plans.Plan, Guid> planRepository,
            IRepository<PlanContentRltAntecedent, Guid> antecedentRepository,
            IGuidGenerator guidGenerator,
            IConstructionDailyAppService dailyAppService,
            ISubItemContentAppService subItemContentAppService,
            IMasterPlanContentAppService masterPlanContentAppService,
            IDistributedCache<List<PlanContent>> cachePlanContent = null, IDistributedCache<Plans.Plan> currentPlan = null) : base(repository)
        {
            _repository = repository;
            _dailyRepo = dailyRepo;
            _planRepository = planRepository;
            _dailyAppService = dailyAppService;
            _antecedentRepository = antecedentRepository;
            _subItemContentAppService = subItemContentAppService;
            _masterPlanContentAppService = masterPlanContentAppService;
            _guidGenerator = guidGenerator;
            repository.GetListAsync();
            _cachePlanContent = cachePlanContent;
            _currentPlan = currentPlan;
        }

        /// <summary>
        /// 更新前的数据验证
        /// </summary>
        protected override void MapToEntity(PlanContentUpdateDto updateInput, PlanContent entity)
        {
            Console.WriteLine("更新前验证数据");
            if (updateInput.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            base.MapToEntity(updateInput, entity);
        }
        /// <summary>
        /// 创建前的数据验证
        /// </summary>
        protected override PlanContent MapToEntity(PlanContentCreateDto updateInput)
        {
            Console.WriteLine("创建前验证数据");
            if (updateInput.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            return base.MapToEntity(updateInput);
        }

        public override async Task<PlanContentDto> CreateAsync(PlanContentCreateDto input)
        {
            if (input.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            PlanContent planContent = ObjectMapper.Map<PlanContentCreateDto, PlanContent>(input);
            // 先 根据 PlanId  获取 树
            List<PlanContent> tree = Repository.Where(x => x.PlanId == input.PlanIdMark).ToList();
            // 展开 树
            List<PlanContent> flattenList = PlanUtil.Flatten(tree);
            if (flattenList.Any(x => x.Name == planContent.Name.Trim()))
            {
                throw new UserFriendlyException($"{planContent.Name}已存在!");
            }
            PlanContent insertAsync = await _repository.InsertAsync(planContent, true);
            PlanContent entity = await _repository.GetAsync(insertAsync.Id);
            entity.Antecedents = input.PreTaskIds
                .Select(x => new PlanContentRltAntecedent(GuidGenerator.Create())
                {
                    PlanContentId = entity.Id,
                    FrontPlanContentId = x
                })
                .ToList();
            await _repository.UpdateAsync(entity);
            return ObjectMapper.Map<PlanContent, PlanContentDto>(entity);
        }
        public async Task<bool> Test(PlanContentCreateDto input)
        {
            DateTime inputTime = input.Time;
            Console.WriteLine(inputTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return false;

        }

        /// <summary>
        /// 获取指定计划关联的所有设备信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<List<AnalogDto>> GetAllRelationEquipment(Guid id)
        {
            var res = new List<AnalogDto>();
            var content = await _cachePlanContent.GetOrAddAsync(id.ToString(), async () => await GetPlanContent(id), () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(3)
            });

            if (content != null && content.Any())
            {
                // 
                content?.ForEach(a =>
                {
                    // 根据计划内容查询关联的设备信息
                    var equipment = a.PlanMaterials?.SelectMany(a => a.PlanMaterialRltEquipments.Select(a => a.Equipment)).ToList();
                    var equipmentDtos = equipment?.Select(a => new AnalogDto { EquipmentName = a.Name, Group = a.Group.Name });
                    if (equipmentDtos != null && equipmentDtos.Any())
                    {
                        res.AddRange(equipmentDtos);
                    }
                });
            }

            return res;
        }

        // 获取进度模拟的数据
        // 实现逻辑 根据计划查询在时间段内的任务内容
        // 查询任务关联的工程量关联设备信息

        public async Task<List<AnalogDto>> GetAnalogData(AnalogInputDto input)
        {
            var res = new List<AnalogDto>();
            // 获取计划的所有内容,将获取的计划内容存放到缓存中，避免多次查询
            var plan = await _currentPlan.GetOrAddAsync(input.PlanId.ToString(), async () => await _planRepository.GetAsync(input.PlanId), () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(3)
            });
            var content = await _cachePlanContent.GetOrAddAsync(input.PlanId.ToString(), async () => await GetPlanContent(input.PlanId), () => new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(3)
            });

            // 获取计划中包含几年
            var year = plan.PlanStartTime.Year;


            if (content != null && content.Any())
            {
                // 根据时间条件进行查询
                var date = GetPlanDate(input, year);
                // 查询服务条件的计划内容
                var resContent = content.Where(a => a.PlanStartTime >= date.startTime && a.PlanStartTime <= date.endTime).ToList();
                resContent?.ForEach(a =>
                {
                    // 根据计划内容查询关联的设备信息
                    var equipment = a.PlanMaterials?.SelectMany(a => a.PlanMaterialRltEquipments.Select(a => a.Equipment)).ToList();
                    var equipmentDtos = equipment?.Select(a => new AnalogDto { EquipmentName = a.Name, Group = a.Group.Name });
                    if (equipmentDtos != null && equipmentDtos.Any())
                    {
                        res.AddRange(equipmentDtos);
                    }
                });
            }
            return res;

        }
        /// <summary>
        /// 获取计划的查询时间
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <param name="sYear">开始年份</param>
        /// <param name="eYear">结束年份</param>
        /// <returns></returns>
        private (DateTime startTime, DateTime endTime) GetPlanDate(AnalogInputDto dto, int year)
        {
            DateTime startTime = DateTime.MinValue, endTime = DateTime.MinValue;
            switch (dto.Type)
            {
                case AnalogType.Day:
                    // 类型为天时前端传的值是时间
                    startTime = DateTime.Parse(dto.Date);
                    endTime = startTime.AddDays(1);
                    break;
                case AnalogType.Week:
                    // 周计划，传过来的时间条件是周
                    var date = GetDateByWeek(year, dto.Speed);
                    startTime = date.startTime;
                    endTime = date.endTime;
                    break;
                case AnalogType.Month:
                    // 查询格式位：2020-02;
                    startTime = DateTime.Parse(dto.Date);
                    endTime = startTime.AddMonths(1).AddDays(-1); // 月的最后一天
                    break;
                case AnalogType.Quarter:
                    // 季度依然是一个值
                    var q1 = new DateTime(year, 1, 1);
                    startTime = q1.AddMonths(dto.Speed * 3 - 3);
                    endTime = startTime.AddMonths(3).AddDays(-1);// 前一天
                    break;
                case AnalogType.HalfYear:
                    startTime = new DateTime(year, 1, 1);
                    endTime = startTime.AddYears(1).AddDays(-1);
                    break;
                default:
                    break;
            }
            return (startTime, endTime);
        }
        /// <summary>
        /// 获取指定年份，指定周数的日期范围
        /// </summary>
        /// <param name="year"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        private (DateTime startTime, DateTime endTime) GetDateByWeek(int year, int week)
        {
            DateTime sDate, eDate;
            var stime = new DateTime(year, 1, 1);
            var etime = new DateTime(year, 12, 31);
            var sw = (int)stime.DayOfWeek;
            if (week == 1)
            {
                sDate = stime;
                eDate = stime.AddDays(6 - sw);
            }
            else
            {
                sDate = stime.AddDays((7 - sw) + (week - 2) * 7);
                eDate = sDate.AddDays(6);
                if (eDate > etime)
                {
                    eDate = etime;
                }
            }
            return (sDate, eDate);
        }



        // 获取计划内容
        private Task<List<PlanContent>> GetPlanContent(Guid planId)
        {
            var tree = Repository.WithDetails().Where(x => x.PlanId == planId).ToList();
            // 展开 树
            List<PlanContent> flattenList = PlanUtil.Flatten(tree);
            return Task.FromResult(flattenList);
        }

        /// <summary>
        /// 在 getList 方法 前 构造 query.where 重写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<PlanContent> CreateFilteredQuery(PlanContentSearchDto input)
        {
            IQueryable<PlanContent> query = Repository.WithDetails(); // include 关联查询,在 对应的 EntityFrameworkCoreModule.cs 文件里面 设置 include 

            // 这里自己手动 写吧,实在是拼不动了…… 
            // query = query
            // 	.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) ||x.Content.Contains(input.SearchKey)  ) // 模糊查询
            // 	.WhereIf(input.ProjectId.HasValue, x => x.ProjectId == input.ProjectId); // 根据xx类型查询 

            return query;
        }
        /// <summary>
        /// 删除多个 施工计划详情
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            await Repository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }


        /// <summary>
        /// 根据 planId 获取年份
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<int>> GetYears(Guid planId)
        {
            List<PlanContent> list = Repository.ToList();
            // 获取 到 树
            List<PlanContent> contents = list.Where(x => x.PlanId == planId).ToList();
            // 把树 展开 
            List<PlanContent> flatArr = PlanUtil.Flatten(contents);
            // 根据 展开 item 的 开始时间 的 年 份 拿出来 返回 
            List<int> dateTimes = flatArr.GroupBy(x => x.PlanStartTime.Year).Select(x => x.Key).ToList();
            return dateTimes;
        }

        public override async Task DeleteAsync(Guid id)
        {
            //先查到,把关联的planId置空,不然一对一的话报 外键id已存在 
            PlanContent planContent = await _repository.GetAsync(id);
            planContent.PlanId = null;
            await _repository.UpdateAsync(planContent, true);
            await _repository.DeleteAsync(id, true);
        }

        /// <summary>
        /// 升级 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> LvUp(Guid id)
        {
            if (!Repository.Any(x => x.Id == id)) throw new UserFriendlyException($"计划任务详情 不存在 ! id = {id}");
            // 先找到 他
            PlanContent child = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (child.ParentId == null) throw new UserFriendlyException($"{child.Name} 不能升级!");
            // 在找到 他 父亲 
            PlanContent parent = Repository.WithDetails().FirstOrDefault(x => x.Id == child.ParentId);
            if (parent.ParentId == null) throw new UserFriendlyException($"{child.Name} 不能升级!");
            // 把 child 的 pid 改成和 parent 的 pId 一样
            child.ParentId = parent.ParentId;
            await Repository.UpdateAsync(child);
            return true;
        }

        /// <summary>
        /// 降级 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> LvDown(Guid id)
        {
            if (!Repository.Any(x => x.Id == id)) throw new UserFriendlyException($"计划任务详情 不存在 ! id = {id}");
            // 先找到 他
            PlanContent entity = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (entity.ParentId == null) throw new UserFriendlyException($"{entity.Name} 不能降级!");
            // 找到父亲 
            PlanContent parent = Repository.WithDetails().FirstOrDefault(x => x.Id == entity.ParentId);
            if (parent == null) throw new UserFriendlyException($"{entity.Name}  没有父级,不能降级!");
            // 在找到 他 儿子 们
            List<PlanContent> children = Repository.WithDetails().Where(x => x.ParentId == id).ToList();
            if (children.Count == 0) throw new UserFriendlyException("没有子级,不能降级!");
            PlanContent child1 = children.FirstOrDefault();
            // 将 child1 和 entity 的 pid 互换
            Guid? ePId = entity.ParentId;
            entity.ParentId = child1.Id;
            child1.ParentId = ePId;
            await Repository.UpdateAsync(entity);
            await Repository.UpdateAsync(child1);
            return true;
        }

        /// <summary>
        /// 批量保存(来自前段的 甘特图组件) 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> BatchSave(List<PlanContentGanttUpdateDto> modifyList)
        {
            // 重复的列表 todo 这个重复校验 去掉了,看后面 会不会在用 ，先注释了
            // List<string> repeatList = modifyList.GroupBy(x => x.Content).Where(x=>x.Count()>1).Select(x=>x.Key).ToList();
            // if (repeatList.Count > 0)
            // {
            // 	throw new UserFriendlyException($"{repeatList[0]}重复,请检查");
            // }
            if (modifyList.Any(x => x.Name.Length > 20)) throw new UserFriendlyException("名称不能超过20位");
            // 先把 所有 标记 为 添加 的 item 批量添加 
            List<PlanContent> addList = ObjectMapper.Map<List<PlanContentGanttUpdateDto>, List<PlanContent>>(modifyList.Where(x => x.GanttItemState == GanttItemState.Add).ToList());
            addList.ForEach(async entity =>
            {
                await Repository.InsertAsync(entity, true);
            });
            // 添加完以后在查出来,将 前置任务ids 修改下
            List<Guid> addIds = addList.Select(x => x.Id).ToList();
            List<PlanContent> addEntities = Repository.Where(x => addIds.Contains(x.Id)).ToList();
            foreach (PlanContent entity in addEntities)
            {

                PlanContentGanttUpdateDto hit = modifyList.FirstOrDefault(m => m.Id == entity.Id);
                if (hit != null && hit.PreTaskIds.Count > 0) // modifyList 和 添加结果列表 比较 命中的话
                { //映射一下 
                    var _antecedents = new List<PlanContentRltAntecedent>();
                    foreach (var id in hit.PreTaskIds)
                    {
                        _antecedents.Add(new PlanContentRltAntecedent(_guidGenerator.Create())
                        {
                            PlanContentId = entity.Id,
                            FrontPlanContentId = id,
                        });
                    }
                    entity.Antecedents = _antecedents;
                    await Repository.UpdateAsync(entity, true);

                }
            }
            // await _masterPlanContentRepository.UpdateRangeAsync(addEntities); //批量更新 //todo 自定义仓储有问题 后面 要 优化下

            // 然后把 所有 标记 为 编辑  的 item 批量 修改  
            List<PlanContentGanttUpdateDto> editList = modifyList.Where(x => x.GanttItemState == GanttItemState.Edit).ToList();
            List<Guid> editIds = editList.Select(x => x.Id).ToList(); // 标记为编辑的 ids 
                                                                      // 根据 editIds 查出 数据库的 实体列表(个别可能查不到,因为删除或者其他的原因)
            List<PlanContent> editEntities = Repository.Where(x => editIds.Contains(x.Id)).ToList(); //编辑的实体列表
            foreach (PlanContent entity in editEntities)
            {
                PlanContentGanttUpdateDto editDto = editList.FirstOrDefault(dto => dto.Id == entity.Id);
                if (editDto != null)
                {
                    ObjectMapper.Map(editDto, entity); //将 dto 的 属性 更新到entity 里面 去

                    if (editDto.PreTaskIds.Count > 0)
                    {
                        //删除之前数据库中关联的数据
                        await _antecedentRepository.DeleteAsync(x => x.PlanContentId == entity.Id);
                        PlanContentGanttUpdateDto hit = modifyList.FirstOrDefault(m => m.Id == entity.Id);
                        var _antecedents = new List<PlanContentRltAntecedent>();
                        foreach (var id in hit.PreTaskIds)
                        {
                            _antecedents.Add(new PlanContentRltAntecedent(_guidGenerator.Create())
                            {
                                PlanContentId = entity.Id,
                                FrontPlanContentId = id,
                            });
                        }
                        entity.Antecedents = _antecedents;
                    }
                    await Repository.UpdateAsync(entity, true);
                }

            }
            // await _masterPlanContentRepository.UpdateRangeAsync(editEntities);//批量更新  自定义仓储有问题 后面 要 优化下
            // 查找标记为 删除 的 


            List<Guid> delIds = modifyList.Where(x => x.GanttItemState == GanttItemState.Delete).Select(x => x.Id).ToList();
            List<PlanContent> delEntities = Repository.Where(x => delIds.Contains(x.Id)).ToList();
            foreach (PlanContent entity in delEntities)
            {
                entity.PlanId = null;
                await Repository.UpdateAsync(entity, true);
            }
            //再查一遍
            List<PlanContent> delAgainEntities = Repository.Where(x => delIds.Contains(x.Id)).ToList();
            //再执行删除  不这样做 直接 delete 的话 前面 的entity.planId=null , 就白更新了,根本不会更新
            //没办法 只能开个线程来 进行删除 ……  这样 会 更新+删除 ,不然就 只会删除(前面更新不执行)
            await Repository.DeleteAsync(x => delIds.Contains(x.Id));
            return true;
        }

        /// <summary>
        /// 根据 施工计划id 获取 详情树(单树)
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<PlanContentDto> GetSingleTree(Guid planId, PlanContentDateFilterDto filter)
        {
            List<PlanContent> list = Repository.WithDetails().ToList();
            List<PlanContent> masterPlanContents = list.Where(x => x.PlanId == planId).ToList();

            List<PlanContentDto> planContentDtos = ObjectMapper.Map<List<PlanContent>, List<PlanContentDto>>(masterPlanContents);
            // 递归复制了一个数组 
            List<PlanContentDto> contentDtos = PlanUtil.RecUse(planContentDtos,
                (oldItem, newItem) =>
                {
                    // List<Daily> dailies = _dailyRepo.WithDetails().Where(x=>x.Dispatch.DispatchRltPlanContents.Select(y=>y.PlanContentId).Contains(oldItem.Id)).ToList();
                    // // 没有日志说明 没设置工程量 派工单 等等 ,进度给0 
                    // if (dailies.Count!=0)
                    // {
                    // 	Daily daily = dailies[0];
                    // 	// 获取 对应 planContent 的 工程量
                    // 	IEnumerable<PlanMaterial> enumerable = daily.DailyRltPlan.Select(x => x.PlanMaterial);
                    // 	// 只获取 施工计划 详情  对应 对应的工程量
                    // 	List<PlanMaterial> planMaterials = enumerable.Where(x => x.PlanContentId == oldItem.Id).ToList();
                    // 	// 计算下 施工计划 详情 对应 工程量总数  
                    // 	decimal totalCount = planMaterials.Sum(x => x.Quantity);
                    // 	// 获取 截至 今天 以来已完成的 设备数量 总合 (dailyAppService 封装好的方法 )
                    // 	int finishCount  = planMaterials.Select( x=>  _dailyAppService.GetDailyRltPlanMaterial(x.Id).Result ).Sum();
                    // 	// 除一下就是 进度啦
                    // 	newItem.AllProgress = totalCount == 0 ? newItem.AllProgress = 0 : Math.Round((double) (finishCount / totalCount),2) ;
                    // }
                    // else
                    // {
                    // 	newItem.AllProgress = 0;
                    // }
                    // Console.WriteLine($"newItem.AllProgress-{newItem.AllProgress}");
                    newItem.Children = PlanUtil.CalcDateFilter(oldItem, filter);
                });

            return contentDtos;
        }

        /// <summary>
        /// 移动任务(给任务换个pId)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public async Task<bool> MoveTask(Guid id, Guid pId)
        {
            PlanContent planContent = await _repository.GetAsync(id);
            planContent.ParentId = pId;
            await _repository.UpdateAsync(planContent);
            return true;
        }
        /// <summary>
        /// 根据日期(时间段) 获取 设备列表 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetEquipmentsByDate(Guid id, string dateUnit)
        {
            return true;
        }




        /// <summary>
        /// 引用 分部分项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="subItemContentIds"></param>
        /// <returns></returns>
        public async Task<bool> ImportSubItem(Guid id, List<Guid> subItemContentIds)
        {
            if (!Repository.Any(x => x.Id == id)) throw new UserFriendlyException($"计划任务详情 不存在 ! id = {id}");
            PlanContent father = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            // 如果只有 一个,则 找到 这个 分项和树下面的所有 子级 ,往 content 里面塞
            subItemContentIds.ForEach(async subItemContentId =>
            {
                // 获取 分项树
                SubItemContentDto subItemTree = _subItemContentAppService.GetSingleTree(subItemContentId);
                // 根据 分项树 创建 任务树
                Guid genId = GuidGenerator.Create(); // 生成 的id 后面 要复用
                PlanContent masterPlanContentTree = new PlanContent(genId)
                {
                    SubItemContentId = subItemTree.Id,
                    Name = subItemTree.Name,
                    PlanStartTime = father.PlanStartTime,
                    PlanEndTime = father.PlanEndTime,
                    Period = father.Period,
                    ParentId = father.Id,
                    Children = new List<PlanContent>()
                };

                //  分项树递归 同时将 任务树也递归创建 子级
                if (subItemTree.Children.Count > 0)
                {
                    subItemTree.Children.ForEach(subItem =>
                    {
                        Rec(subItem, masterPlanContentTree, father, genId);
                    });

                }

                await Repository.InsertAsync(masterPlanContentTree);

            });

            return true;
        }

        /// <summary>
        /// 引用总体计划 
        /// </summary>
        /// <param name="planId"></param>
        /// <param name="masterPlanId"></param>
        /// <returns></returns>
        public async Task<bool> ImportMasterPlan(Guid planId, Guid masterPlanId)
        {

            Plans.Plan plan = await _planRepository.GetAsync(planId);
            List<MasterPlanContentDto> masterPlanTree = _masterPlanContentAppService.GetSingleTree(masterPlanId, new PlanContentDateFilterDto());

            // 内置一个转换方法 
            PlanContent ConvertToPlanContent(Guid guid, MasterPlanContentDto masterPlanContentDto, Guid? pId, Guid? planId)
            {
                PlanContent newEntity = new PlanContent(guid)
                {
                    PlanId = planId,
                    Name = masterPlanContentDto.Name,
                    Content = masterPlanContentDto.Content,
                    PlanStartTime = masterPlanContentDto.PlanStartTime,
                    PlanEndTime = masterPlanContentDto.PlanEndTime,
                    Period = masterPlanContentDto.Period,
                    IsMilestone = masterPlanContentDto.IsMilestone,
                    ParentId = pId,
                };
                return newEntity;
            }
            // 内置一个递归方法方法 
            void RecInsert(List<MasterPlanContentDto> list, Guid pId)
            {
                list.ForEach(async masterPlanContentDto =>
                {
                    Guid guid = GuidGenerator.Create(); //生成一个guid 
                    PlanContent newEntity = ConvertToPlanContent(guid, masterPlanContentDto, pId, null);
                    await Repository.InsertAsync(newEntity); //递归添加
                    List<MasterPlanContentDto> children = masterPlanContentDto.Children;
                    if (children.Count > 0)
                    {
                        RecInsert(children, guid);
                    }
                });
            }

            // 递归保存 
            masterPlanTree.ForEach(async masterPlanContentDto =>
            {
                Guid guid = GuidGenerator.Create(); //生成一个guid 

                PlanContent newEntity = ConvertToPlanContent(guid, masterPlanContentDto, null, planId);

                // 因为 planContent 的 id 私有 ,不能修改 ,所以只能创建个带id 的新实体 ,然后 把 缺 的属性 (除id 外) 填充到这个新 实体里面 
                // newEntity.PlanId = plan.Id; 
                await Repository.InsertAsync(newEntity);
                List<MasterPlanContentDto> children = masterPlanContentDto.Children;
                if (children.Count > 0)
                {
                    RecInsert(children, guid);
                }

            });

            return true;
        }
        private void Rec(SubItemContentDto subItemContentDto, PlanContent masterPlanContent, PlanContent father, Guid pId)
        {
            Guid genId = GuidGenerator.Create();
            masterPlanContent.Children.Add(
                new PlanContent(genId)
                {
                    SubItemContentId = subItemContentDto.Id,
                    Name = subItemContentDto.Name,
                    PlanStartTime = father.PlanStartTime,
                    PlanEndTime = father.PlanEndTime,
                    Period = father.Period,
                    ParentId = pId,
                    Children = new List<PlanContent>()
                }
            );
            if (subItemContentDto.Children.Count > 0)
            {
                subItemContentDto.Children.ForEach(x =>
                {
                    Rec(x, masterPlanContent, father, genId);
                });
            }
        }

    }
}
