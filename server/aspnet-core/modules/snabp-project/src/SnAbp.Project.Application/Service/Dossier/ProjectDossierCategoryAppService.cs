using SnAbp.Project.Dtos;
using SnAbp.Project.Entities;
using SnAbp.Project.IServices.Dossier;
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
    public class ProjectDossierCategoryAppService : ProjectAppService, IProjectDossierCategoryAppService
    {
        private readonly IRepository<Archives, Guid> _archivesRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ArchivesCategory, Guid> _archivesCategoryRepository;
        public ProjectDossierCategoryAppService(
                 IGuidGenerator guidGenerator,
                 IRepository<Archives, Guid> archivesRepository,
            IRepository<ArchivesCategory, Guid> archivesCategoryRepository
                 )
        {
            _archivesRepository = archivesRepository;
            _archivesCategoryRepository = archivesCategoryRepository;
            _guidGenerator = guidGenerator;
        }
        /// <summary>
        /// 获取卷宗分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<List<FileClassificationDto>> GetList(FileClassificationSearchDto input)
        {

            //1、获取所有的档案
            var archivesCategory = _archivesCategoryRepository.WithDetails(x => x.Children)
                .WhereIf(input.ParentId != null && input.ParentId != Guid.Empty, x => x.ParentId == input.ParentId)
            .WhereIf(input.ParentId == null || input.ParentId == Guid.Empty, x => x.ParentId == null);
            var res = ObjectMapper.Map<List<ArchivesCategory>, List<FileClassificationDto>>(archivesCategory.OrderBy(x => x.Name).ToList());

            //2、获取所有的卷宗，作为对应的子级
            GetChildrenArchives(res);
            //3、查找卷宗
            var archives = _archivesRepository.WithDetails().Where(x => x.ArchivesCategoryId == input.ParentId).ToList();
            var fileClassificationChild = ObjectMapper.Map<List<Archives>, List<FileClassificationDto>>(archives.OrderBy(x => x.Name).ToList());
            foreach (var child in fileClassificationChild)
            {
                child.IsFile = true;
                child.ParentId = input.ParentId;
                res.Add(child);

            }
            foreach (var item in res)
            {
                if (item.IsFile==true || item.Children.Count == 0)
                {
                    item.Children = null;
                  
                }
                else
                {
                    item.Children = new List<FileClassificationDto>();
                }
            }
            return Task.FromResult(res);
        }
        #region  私有方法
        List<FileClassificationDto> GetChildren(List<FileClassificationDto> data, Guid? id)
        {
            var fileClassifications = new List<FileClassificationDto>();
            var children = data.Where(x => x.ParentId == id);
            foreach (var item in children)
            {

                var node = new FileClassificationDto(item.Id);
                node.Name = item.Name;
                node.ParentId = item.ParentId;
                node.IsFile = item.IsFile;
                node.Children = GetChildren(data, item.Id);
                fileClassifications.Add(node);
            }
            return fileClassifications;
        }
        bool GetChildrenArchives(List<FileClassificationDto> data)
        {
            var fileClassifications = new List<FileClassificationDto>();
            var archivesList = _archivesRepository.WithDetails();
            foreach (var item in data)
            {
                var archives = archivesList.Where(x => x.ArchivesCategoryId == item.Id).ToList();
                if (archives.Count == 0) continue;
                var fileClassificationChild = ObjectMapper.Map<List<Archives>, List<FileClassificationDto>>(archives.OrderBy(x => x.Name).ToList());
                foreach (var child in fileClassificationChild)
                {
                    child.IsFile = true;
                    child.ParentId = item.Id;
                    item.Children.Add(child);
                  
                }
            }
            return true;
        }
        #endregion
    }
}