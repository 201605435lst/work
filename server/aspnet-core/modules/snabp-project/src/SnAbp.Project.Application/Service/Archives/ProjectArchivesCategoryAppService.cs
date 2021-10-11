using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
using SnAbp.Project.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Project.Service
{
    public class ProjectArchivesCategoryAppService : ProjectAppService, IProjectArchivesCategoryAppService
    {
        private readonly IRepository<ArchivesCategory, Guid> _archivesCategoryRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ProjectArchivesCategoryAppService(
            IGuidGenerator guidGenerator,
            IRepository<ArchivesCategory, Guid> archivesCategoryRepository
            )
        {
            _archivesCategoryRepository = archivesCategoryRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ArchivesCategoryDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的档案分类");
            var archivesCategory = _archivesCategoryRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (archivesCategory == null) throw new UserFriendlyException("当前档案分类不存在");
            var result = ObjectMapper.Map<ArchivesCategory, ArchivesCategoryDto>(archivesCategory);
            return Task.FromResult(result);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<ArchivesCategoryDto>> GetList(ArchivesSearchDto input)
        {
            var result = new PagedResultDto<ArchivesCategoryDto>();
            var archivesCategory = _archivesCategoryRepository.WithDetails(x => x.Children)
                
                .WhereIf(!String.IsNullOrEmpty(input.Name), x => x.Name.Contains(input.Name));
            var archivesCategoryTree = GetChildren(archivesCategory.ToList(), null);
            var res = ObjectMapper.Map<List<ArchivesCategory>, List<ArchivesCategoryDto>>(archivesCategoryTree.OrderBy(x => x.Order).ToList());
            SetChildrenNull(res);
            return Task.FromResult(res);
        }
        /// <summary>
        /// 创建信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ArchivesCategoryDto> Create(ArchivesCategoryCreateDto input)
        {
            ArchivesCategoryDto archivesCategoryDto = new ArchivesCategoryDto();
            var archivesCategory = new ArchivesCategory();
            CheckSameName(input.Name, null, input.ParentId);
            ObjectMapper.Map(input, archivesCategory);
            archivesCategory.SetId(_guidGenerator.Create());
            await _archivesCategoryRepository.InsertAsync(archivesCategory);
            archivesCategoryDto = ObjectMapper.Map<ArchivesCategory, ArchivesCategoryDto>(archivesCategory);
            return archivesCategoryDto;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ArchivesCategoryDto> Update(ArchivesCategoryUpdateDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要修改的档案分类");
            var archivesCategory = await _archivesCategoryRepository.GetAsync(input.Id);
            CheckSameName(input.Name, input.Id, input.ParentId);

            if (archivesCategory == null) throw new UserFriendlyException("当前档案分类不存在");
            ObjectMapper.Map(input, archivesCategory);
            await _archivesCategoryRepository.UpdateAsync(archivesCategory);
            return ObjectMapper.Map<ArchivesCategory, ArchivesCategoryDto>(archivesCategory);
        }
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Lock(ArchivesCategoryLockDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) throw new UserFriendlyException("请确定要加密的档案分类");
            var archivesCategory = await _archivesCategoryRepository.GetAsync(input.Id);
            if (archivesCategory == null) throw new UserFriendlyException("当前档案分类不存在");
            archivesCategory.IsEncrypt = input.IsEncrypt;
              await _archivesCategoryRepository.UpdateAsync(archivesCategory);
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _archivesCategoryRepository.WithDetails(x=>x.Children).FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此档案分类不存在");
            if (ent.Children != null && ent.Children.Count > 0) throw new UserFriendlyException("请先删除下级档案分类！！");
            await _archivesCategoryRepository.DeleteAsync(id);
            return true;
        }
        #region
        bool CheckSameName(string Name, Guid? id, Guid? parentId)
        {
            var archivesCategory = _archivesCategoryRepository.WithDetails()
                .Where(x => x.Name.ToUpper() == Name.ToUpper());
            if (parentId != null && parentId != Guid.Empty)
            {
                archivesCategory = archivesCategory.Where(x => x.ParentId == parentId);
            }
            else
            {
                archivesCategory = archivesCategory.Where(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }

            if (id.HasValue)
            {
                archivesCategory = archivesCategory.Where(x => x.Id != id.Value);
            }

            if (archivesCategory.Count() > 0)
            {
                throw new UserFriendlyException("当前分类中已存在相同名字的档案名!!!");
            }

            return true;
        }
        List<ArchivesCategory> GetChildren(List<ArchivesCategory> data, Guid? id)
        {
            var archivesCategory = new List<ArchivesCategory>();
            var children = data.Where(x => x.ParentId == id);
            foreach (var item in children)
            {
                var node = new ArchivesCategory(item.Id);
                node.Name = item.Name;
                node.Order = item.Order;
                node.ParentId = item.ParentId;
                node.IsEncrypt = item.IsEncrypt;
                node.Remark = item.Remark;
                node.Children = GetChildren(data, item.Id);
                archivesCategory.Add(node);
            }
            return archivesCategory;
        }
        List<ArchivesCategoryDto> SetChildrenNull(List<ArchivesCategoryDto> data)
        {
            foreach (var item in data)
            {
                if (item.Children.Count == 0)
                {
                    item.Children = null;
                }
                else
                {
                    SetChildrenNull(item.Children);
                }
            }
            return data;
        }


        #endregion
    }
}
