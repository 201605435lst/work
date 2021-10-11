using AutoMapper;
using SnAbp.Exam.Dtos;
using SnAbp.Exam.Entities;
using SnAbp.Exam.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Utils.TreeHelper;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Microsoft.AspNetCore.Authorization;
using SnAbp.Exam.Authorization;

namespace SnAbp.Exam.Services
{
    [Authorize]
    public class ExamKnowledgePointAppService : ExamAppService, IExamKnowledegPointSearchAppService
    {
        private readonly IRepository<KnowledgePoint, Guid> _knowledgePointIRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<KnowledgePointRltCategory, Guid> _categories;

        public ExamKnowledgePointAppService(
            IRepository<KnowledgePoint, Guid> knowledgePointRep,
            IGuidGenerator guidGenerator,
            IRepository<KnowledgePointRltCategory,Guid > categories

            )
        {
            _knowledgePointIRepository = knowledgePointRep;
            _guidGenerator = guidGenerator;
            _categories = categories;
        }


        [Authorize(ExamPermissions.KnowledgePoint.Create)]
        public async Task<KnowledgePointDto> Create(KnowledgePointCreateDto dto)
        {
           

            if (string.IsNullOrEmpty(dto.Name.Trim())) throw new UserFriendlyException("请输入名称");
            
            foreach(var id in dto.CategoryIds)
            {
                if(dto.CategoryIds == null|| dto.CategoryIds.Count==0) throw new UserFriendlyException("分类不能为空");
            }

            CheckSameName(dto.Name, null, dto.ParentId);

            var knowledgePointId = _guidGenerator.Create();
            var knowledgePoint = new KnowledgePoint(knowledgePointId);

            knowledgePoint.Name = dto.Name;
            knowledgePoint.ParentId = dto.ParentId;
            knowledgePoint.Order = dto.Order;
            knowledgePoint.Description = dto.Description;
            knowledgePoint.KnowledgePointRltCategories = new List<KnowledgePointRltCategory>();

            foreach (var categoryId in dto.CategoryIds)
            {
                knowledgePoint.KnowledgePointRltCategories.Add(new KnowledgePointRltCategory(Guid.NewGuid())
                {
                    CategoryId = categoryId,
                }); 
            }
            await _knowledgePointIRepository.InsertAsync(knowledgePoint);

            return ObjectMapper.Map<KnowledgePoint, KnowledgePointDto>(knowledgePoint);
        }

        [Authorize(ExamPermissions.KnowledgePoint.Update)]
        public async Task<KnowledgePointDto> Update(KnowledegPointUpdateDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name)) throw new UserFriendlyException("名称不能为空");

            foreach (var id in dto.CategoryIds)
            {
                if (dto.CategoryIds == null || dto.CategoryIds.Count == 0) throw new UserFriendlyException("分类不能为空");
            }
            if (dto.ParentId.HasValue && dto.ParentId.Value == dto.Id)
            {
                throw new UserFriendlyException("知识点父级不能修改知识点本身");
            }

            //重复名称检查
            CheckSameName(dto.Name, dto.Id, dto.ParentId);
            var knowledgePoint = await _knowledgePointIRepository.GetAsync(dto.Id);

            if (knowledgePoint == null)
            {
                throw new UserFriendlyException("该知识点不存在");
            }


            var knowledgePointsRltCategory = _categories.Where(x => x.KnowledgePointId == dto.Id);
            //清除之前关联关系
            await _categories.DeleteAsync(x => x.KnowledgePointId == knowledgePoint.Id);

            //重新保存关联关系
            knowledgePoint.KnowledgePointRltCategories = new List<KnowledgePointRltCategory>();

            foreach(var category in dto.CategoryIds)
            {
                knowledgePoint.KnowledgePointRltCategories.Add(new KnowledgePointRltCategory(Guid.NewGuid())
                {
                    CategoryId = category,
                });
            }

            knowledgePoint.Name = dto.Name;
            knowledgePoint.Description = dto.Description;
            knowledgePoint.Order = dto.Order;
            knowledgePoint.ParentId = dto.ParentId;
            


            await _knowledgePointIRepository.UpdateAsync(knowledgePoint);

            return ObjectMapper.Map<KnowledgePoint, KnowledgePointDto>(knowledgePoint);


        }

        [Authorize(ExamPermissions.KnowledgePoint.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确id");
            var ent = await _knowledgePointIRepository.GetAsync(id);
            if (ent == null) throw new UserFriendlyException("该知识点不存在");

            var knowledgePointIRepository = _knowledgePointIRepository.Where(x => id == ent.Id).ToList();
            var data = GetChildren(knowledgePointIRepository, id);
            if (data.Count() > 0) throw new UserFriendlyException("存在下级分类无法删除");

            await _knowledgePointIRepository.DeleteAsync(id);
            return true;
        }

        //获得子节点
        private List<KnowledgePoint> GetChildren(IEnumerable<KnowledgePoint> data, Guid? parentId)
        {
            List<KnowledgePoint> list = new List<KnowledgePoint>();
            var children = data.Where(o => o.ParentId == parentId);
            foreach (var item in children)
            {
                var node = new KnowledgePoint(item.Id);
                node.Name = item.Name;
                node.Description = item.Description;
                node.Order = item.Order;
                node.Parent = item.Parent;
                node.ParentId = item.ParentId;
                node.CreationTime = item.CreationTime;
                node.KnowledgePointRltCategories = item.KnowledgePointRltCategories;
                node.Children = GetChildren(data, node.Id);
                list.Add(node);
            }
            return list;

        }

        public Task<KnowledgePointDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("id is null");

            var knowledgePoint = _knowledgePointIRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (knowledgePoint == null) throw new UserFriendlyException("该知识点不存在");

            return Task.FromResult(ObjectMapper.Map<KnowledgePoint, KnowledgePointDto>(knowledgePoint));
        }

        public Task<PagedResultDto<KnowledgePointDto>> GetList(KnowledegPointSearchDto dto)
        {
            //根据知识点名称、分类名称查询 
            var knowledge = _knowledgePointIRepository.WithDetails()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.Contains(dto.Name))
                .WhereIf(dto.CategoryIds != null && dto.CategoryIds.Count > 0, x => x.KnowledgePointRltCategories.Any(y => dto.CategoryIds.Contains(y.CategoryId)));
                

            var filteredKnowledge = GetChildren(knowledge.ToList(), null);
            var result = new PagedResultDto<KnowledgePointDto>();
            result.TotalCount = filteredKnowledge.Count();

            if (dto.IsAll == false)
            {
                filteredKnowledge = filteredKnowledge.OrderByDescending(x=>x.CreationTime).Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
            }
            result.Items = ObjectMapper.Map<List<KnowledgePoint>, List<KnowledgePointDto>>(filteredKnowledge);
            return Task.FromResult(result);
        }

        private bool CheckSameName(string name, Guid? id, Guid? parentId)
        {
            var sameName = _knowledgePointIRepository.Where(o => o.Name.ToUpper() == name.ToUpper());

            if (parentId != null && parentId != Guid.Empty)
            {
                sameName = sameName.Where(x => x.ParentId == parentId);
            }
            else
            {
                sameName = sameName.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (id.HasValue)
            {
                sameName = sameName.Where(o => o.Id != id.Value);
            }
            if (sameName.Count() > 0)
            {
                throw new UserFriendlyException("知识点已存在");
            }
            return true;
        }


        //判断同一分类是否存在相同知识点

        private bool CkeckSameCategory(Guid? categoryId, Guid? id, Guid? parentId)
        {
           /*var sameCategory = _knowledgePointIRepository.Where(o => o.KnowledgePointRltCategories..Id != categoryId && o.ParentId == parentId);
            if (id.HasValue)
            {
                sameCategory = sameCategory.Where(x => x.Id != id.Value);
            }
            if (sameCategory.Count() > 0)
            {
                throw new UserFriendlyException("不可选择父节点以外分类");
            }*/
            return true;
        }

        public async Task<List<KnowledgePointDto>> GetTreeList(Guid? parentId)
        {
            //throw new NotImplementedException();
            var list = await _knowledgePointIRepository.GetListAsync();
            list = GuidKeyTreeHelper<KnowledgePoint>.GetTree(list);
            return ObjectMapper.Map<List<KnowledgePoint>, List<KnowledgePointDto>>(list);
        }

        public Task<List<KnowledgePointDto>> GetByParentId(Guid? parentId)
        {

            //throw new NotImplementedException();
           /* var componentCategories = _repositoryComponentCategory
                .WhereIf(parentId != null || parentId != Guid.Empty, x => x.ParentId == parentId)
                .WhereIf(parentId == null || parentId == Guid.Empty, x => x.Parent == null) // 顶级
                .ToList();

            return Task.FromResult(ObjectMapper.Map<List<ComponentCategory>, List<ComponentCategoryDto>>(componentCategories));*/
            if (parentId == null)
                return Task.FromResult(ObjectMapper.Map<List<KnowledgePoint>, List<KnowledgePointDto>>(_knowledgePointIRepository.Where(x => x.Parent == null).ToList()));
            else
                return Task.FromResult(ObjectMapper.Map<List<KnowledgePoint>, List<KnowledgePointDto>>(_knowledgePointIRepository.Where(x => x.ParentId == parentId).ToList()));
        }

        public Task<List<KnowledgePointDto>> GetParentsByIds(List<Guid> ids)
        {
            //throw new NotImplementedException();
            var knowledgePoints = new List<KnowledgePoint>();
           

            if (ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    knowledgePoints.AddRange(GetParents(id, new List<KnowledgePoint>()));
                }
            }
       

            var diffknowledgePoints = knowledgePoints.Distinct().ToList();//去除重复
            diffknowledgePoints.AddRange(_knowledgePointIRepository.Where(x => x.ParentId == null).ToList());
            var treeList = GuidKeyTreeHelper<KnowledgePoint>.GetTree(diffknowledgePoints);
            return Task.FromResult(ObjectMapper.Map<List<KnowledgePoint>, List<KnowledgePointDto>>(treeList));
        }

        private List<KnowledgePoint> GetParents(Guid? id, List<KnowledgePoint> knowledgePoints)
        {
            knowledgePoints.AddRange(_knowledgePointIRepository.Where(a => a.ParentId == id));
            var knowledgePoint = _knowledgePointIRepository.FirstOrDefault(a => a.Id == id);
            if (knowledgePoint.ParentId != null)
            {
                GetParents(knowledgePoint.ParentId, knowledgePoints);
            }
            return knowledgePoints;
        }

    }
}
