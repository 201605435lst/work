using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnAbp.Construction.Dtos.MasterPlan.MasterPlanContent;
using SnAbp.Construction.Dtos.Plan.PlanContent;
using SnAbp.Construction.Enums;
using SnAbp.Construction.IServices;
using SnAbp.Construction.IServices.MasterPlan;
using SnAbp.Construction.MasterPlans.Entities;
using SnAbp.Construction.MasterPlans.IRepositories;
using SnAbp.ConstructionBase.Dtos.SubItem;
using SnAbp.ConstructionBase.IServices;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Construction.Services.MaterPlan
{
    /// <summary>
    /// 施工计划详情 Service 
    /// </summary>
    // [Authorize]
    public class MasterPlanContentAppService : CrudAppService<
            MasterPlanContent,
            MasterPlanContentDto, Guid, MasterPlanContentSearchDto,
            MasterPlanContentCreateDto,
            MasterPlanContentUpdateDto>,
        IMasterPlanContentAppService
    {
        private readonly ISubItemContentAppService _subItemContentAppService;
        private readonly IMasterPlanContentRepository _masterPlanContentRepository;
        private readonly IRepository<MasterPlanRltContentRltAntecedent, Guid> _antecedentRepository;
        private readonly IGuidGenerator _guidGenerator;

        public MasterPlanContentAppService(
            IRepository<MasterPlanContent, Guid> repository,
            ISubItemContentAppService subItemContentAppService,
            IRepository<MasterPlanRltContentRltAntecedent, Guid> antecedentRepository,
            IGuidGenerator guidGenerator,
            IMasterPlanContentRepository masterPlanContentRepository
        ) : base(repository)
        {
            _subItemContentAppService = subItemContentAppService;
            _antecedentRepository = antecedentRepository;
            _guidGenerator = guidGenerator;
            _masterPlanContentRepository = masterPlanContentRepository;
            repository.GetListAsync();
        }

        /// <summary>
        /// 更新前的数据验证
        /// </summary>
        protected override void MapToEntity(MasterPlanContentUpdateDto updateInput, MasterPlanContent entity)
        {
            if (updateInput.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            Console.WriteLine("更新前验证数据");
            base.MapToEntity(updateInput, entity);
        }

        /// <summary>
        /// 创建前的数据验证
        /// </summary>
        protected override MasterPlanContent MapToEntity(MasterPlanContentCreateDto updateInput)
        {
            if (updateInput.Name.Trim().Length > 20) throw new UserFriendlyException("名称不能超过20位");
            Console.WriteLine("创建前验证数据");
            return base.MapToEntity(updateInput);
        }

        public override async Task<MasterPlanContentDto> CreateAsync(MasterPlanContentCreateDto input)
        {
            MasterPlanContent planContent = ObjectMapper.Map<MasterPlanContentCreateDto, MasterPlanContent>(input);
            // 先 根据 masterPlanId  获取 树
            List<MasterPlanContent> tree = Repository.ToList().Where(x => x.MasterPlanId == input.MasterPlanIdMark).ToList();
            // 展开 树
            List<MasterPlanContent> flattenList = PlanUtil.Flatten(tree);
            if (flattenList.Any(x => x.Name == planContent.Name.Trim()))
            {
                throw new UserFriendlyException($"{planContent.Name}已存在!");
            }
            MasterPlanContent insertAsync = await Repository.InsertAsync(planContent, true);
            MasterPlanContent entity = await Repository.GetAsync(insertAsync.Id);
            entity.Antecedents = input.PreTaskIds
                .Select(x => new MasterPlanRltContentRltAntecedent(GuidGenerator.Create())
                {
                    MasterPlanContentId = entity.Id,
                    FrontMasterPlanContentId = x
                })
                .ToList();
            await Repository.UpdateAsync(entity);
            return ObjectMapper.Map<MasterPlanContent, MasterPlanContentDto>(entity);
        }


        /// <summary>
        /// 在 getList 方法 前 构造 query.where 重写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override IQueryable<MasterPlanContent> CreateFilteredQuery(MasterPlanContentSearchDto input)
        {
            IQueryable<MasterPlanContent>
                query = Repository.WithDetails(); // include 关联查询,在 对应的 EntityFrameworkCoreModule.cs 文件里面 设置 include 

            // 这里自己手动 写吧,实在是拼不动了…… 
            // query = query
            // 	.WhereIf(!input.SearchKey.IsNullOrWhiteSpace(), x => x.Name.Contains(input.SearchKey) ||x.Content.Contains(input.SearchKey)  ) // 模糊查询
            // .WhereIf(input.MasterPlanId.HasValue, x => x.MasterPlanId == input.MasterPlanId); // 根据xx类型查询 

            return query;
        }

        /// <summary>
        /// 修改前置任务
        /// </summary>
        /// <param name="id">主id </param>
        /// <param name="contentIds">其他ids </param>
        /// <returns></returns>
        public async Task<bool> ChangeFrontTask(Guid id, List<Guid> contentIds)
        {
            if (!Repository.WithDetails().Any(x => x.Id == id))
            {
                throw new UserFriendlyException("任务计划详情 content 不存在!");
            }
            MasterPlanContent masterPlanContent = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (masterPlanContent != null)
            {
                masterPlanContent.Antecedents = contentIds
                    .Select(x => new MasterPlanRltContentRltAntecedent(GuidGenerator.Create()) { MasterPlanContentId = id, FrontMasterPlanContentId = x })
                    .ToList();
                await Repository.UpdateAsync(masterPlanContent);
            }
            return true;
        }

        public override async Task DeleteAsync(Guid id)
        {
            //先查到,把关联的planId置空,不然一对一的话报 外键id已存在 
            var content = await Repository.GetAsync(id);
            content.MasterPlanId = null;
            await Repository.UpdateAsync(content, true);
            await Repository.DeleteAsync(id, true);
        }
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            await Repository.DeleteAsync(x => ids.Contains(x.Id));

            return true;
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
            MasterPlanContent child = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (child.ParentId == null) throw new UserFriendlyException($"{child.Name} 不能升级!");
            // 在找到 他 父亲 
            MasterPlanContent parent = Repository.WithDetails().FirstOrDefault(x => x.Id == child.ParentId);
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
            MasterPlanContent entity = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            if (entity.ParentId == null) throw new UserFriendlyException($"{entity.Name} 不能降级!");
            // 找到父亲 
            MasterPlanContent parent = Repository.WithDetails().FirstOrDefault(x => x.Id == entity.ParentId);
            if (parent == null) throw new UserFriendlyException($"{entity.Name}  没有父级,不能降级!");
            // 在找到 他 儿子 们
            List<MasterPlanContent> children = Repository.WithDetails().Where(x => x.ParentId == id).ToList();
            if (children.Count == 0) throw new UserFriendlyException("没有子级,不能降级!");
            MasterPlanContent child1 = children.FirstOrDefault();
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
        public async Task<bool> BatchSave(List<MasterPlanContentGanttUpdateDto> modifyList)
        {
            // 重复的列表 todo 这个重复校验 去掉了,看后面 会不会在用 ，先注释了
            // List<string> repeatList = modifyList.GroupBy(x => x.Content).Where(x=>x.Count()>1).Select(x=>x.Key).ToList();
            // if (repeatList.Count > 0)
            // {
            // 	throw new UserFriendlyException($"{repeatList[0]}重复,请检查");
            // }
            if (modifyList.Any(x => x.Name.Length > 20)) throw new UserFriendlyException("名称不能超过20位");
            // 先把 所有 标记 为 添加 的 item 批量添加 
            List<MasterPlanContent> addList = ObjectMapper.Map<List<MasterPlanContentGanttUpdateDto>, List<MasterPlanContent>>(modifyList.Where(x => x.GanttItemState == GanttItemState.Add).ToList());
            // await _masterPlanContentRepository.AddRangeAsync(addList);
            foreach (MasterPlanContent entity in addList)
            {
                await Repository.InsertAsync(entity, true);
            }
            // 添加完以后在查出来,将 前置任务ids 修改下
            List<Guid> addIds = addList.Select(x => x.Id).ToList();
            List<MasterPlanContent> addEntities = Repository.Where(x => addIds.Contains(x.Id)).ToList();
            foreach (MasterPlanContent entity in addEntities)
            {
                MasterPlanContentGanttUpdateDto hit = modifyList.FirstOrDefault(m => m.Id == entity.Id);
                if (hit != null) // modifyList 和 添加结果列表 比较 命中的话
                { //映射一下 
                    entity.Antecedents = hit.PreTaskIds.Select(p => new MasterPlanRltContentRltAntecedent(GuidGenerator.Create()) { MasterPlanContentId = entity.Id, FrontMasterPlanContentId = p }).ToList();
                    await Repository.UpdateAsync(entity, true);

                }
            }
            // await _masterPlanContentRepository.UpdateRangeAsync(addEntities); //批量更新 //todo 自定义仓储有问题 后面 要 优化下

            // 然后把 所有 标记 为 编辑  的 item 批量 修改  
            List<MasterPlanContentGanttUpdateDto> editList = modifyList.Where(x => x.GanttItemState == GanttItemState.Edit).ToList();
            List<Guid> editIds = editList.Select(x => x.Id).ToList(); // 标记为编辑的 ids 
                                                                      // 根据 editIds 查出 数据库的 实体列表(个别可能查不到,因为删除或者其他的原因)
            List<MasterPlanContent> editEntities = Repository.Where(x => editIds.Contains(x.Id)).ToList(); //编辑的实体列表
            foreach (MasterPlanContent entity in editEntities)
            {
                MasterPlanContentGanttUpdateDto editDto = editList.FirstOrDefault(dto => dto.Id == entity.Id);
                if (editDto != null)
                {
                    ObjectMapper.Map(editDto, entity); //将 dto 的 属性 更新到entity 里面 去
                    if (editDto.PreTaskIds.Count > 0)
                    {
                        //删除之前数据库中关联的数据
                        await _antecedentRepository.DeleteAsync(x => x.MasterPlanContentId == entity.Id);
                        MasterPlanContentGanttUpdateDto hit = modifyList.FirstOrDefault(m => m.Id == entity.Id);
                        var _antecedents = new List<MasterPlanRltContentRltAntecedent>();
                        foreach (var id in hit.PreTaskIds)
                        {
                            _antecedents.Add(new MasterPlanRltContentRltAntecedent(_guidGenerator.Create())
                            {
                                MasterPlanContentId = entity.Id,
                                FrontMasterPlanContentId = id,
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
            await Repository.DeleteAsync(x => delIds.Contains(x.Id));
            return true;
        }

        /// <summary>
        /// 根据 masterPlanId 获取年份
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<int>> GetYears(Guid masterPlanId)
        {
            List<MasterPlanContent> list = Repository.ToList();
            List<MasterPlanContent> contents = list.Where(x => x.MasterPlanId == masterPlanId).ToList();
            List<MasterPlanContent> flatArr = PlanUtil.Flatten(contents);
            List<int> dateTimes = flatArr.GroupBy(x => x.PlanStartTime.Year).Select(x => x.Key).ToList();
            return dateTimes;
        }

        /// <summary>
        /// 根据 施工计划id 获取 详情树(单树)
        /// </summary>
        /// <param name="masterPlanId"></param>
        /// <returns></returns>
        public List<MasterPlanContentDto> GetSingleTree(Guid masterPlanId, PlanContentDateFilterDto filter)
        {
            List<MasterPlanContent> list = Repository.WithDetails().ToList();
            List<MasterPlanContent> masterPlanContents = list.Where(x => x.MasterPlanId == masterPlanId).ToList();
            List<MasterPlanContentDto> masterPlanContentDtos = ObjectMapper.Map<List<MasterPlanContent>, List<MasterPlanContentDto>>(masterPlanContents);

            List<MasterPlanContentDto> contentDtos = PlanUtil.RecUse(masterPlanContentDtos,
                (oldItem, newItem) =>
                {
                    // 根据日期筛选
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
            MasterPlanContent masterPlanContent = await Repository.GetAsync(id);
            masterPlanContent.ParentId = pId;
            await Repository.UpdateAsync(masterPlanContent);
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
            MasterPlanContent father = Repository.WithDetails().FirstOrDefault(x => x.Id == id);
            // 如果只有 一个,则 找到 这个 分项和树下面的所有 子级 ,往 content 里面塞
            subItemContentIds.ForEach(async subItemContentId =>
            {
                // 获取 分项树
                SubItemContentDto subItemTree = _subItemContentAppService.GetSingleTree(subItemContentId);
                // 根据 分项树 创建 任务树
                Guid genId = GuidGenerator.Create(); // 生成 的id 后面 要复用
                MasterPlanContent masterPlanContentTree = new MasterPlanContent(genId)
                {
                    SubItemContentId = subItemTree.Id,
                    Name = subItemTree.Name,
                    PlanStartTime = father.PlanStartTime,
                    PlanEndTime = father.PlanEndTime,
                    Period = father.Period,
                    ParentId = father.Id,
                    Children = new List<MasterPlanContent>()
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

        private void Rec(SubItemContentDto subItemContentDto, MasterPlanContent masterPlanContent, MasterPlanContent father, Guid pId)
        {
            Guid genId = GuidGenerator.Create();
            masterPlanContent.Children.Add(
                new MasterPlanContent(genId)
                {
                    SubItemContentId = subItemContentDto.Id,
                    Name = subItemContentDto.Name,
                    PlanStartTime = father.PlanStartTime,
                    PlanEndTime = father.PlanEndTime,
                    Period = father.Period,
                    ParentId = pId,
                    Children = new List<MasterPlanContent>()
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