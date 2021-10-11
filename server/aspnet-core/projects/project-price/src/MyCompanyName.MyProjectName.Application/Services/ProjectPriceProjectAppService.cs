using MyCompanyName.MyProjectName.Dtos;
using MyCompanyName.MyProjectName.Entities;
using MyCompanyName.MyProjectName.IServices;
using MyCompanyName.MyProjectName.TemplateModel;
using SnAbp.Utils.DataImport;
using SnAbp.Utils.ExcelHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace MyCompanyName.MyProjectName.Services
{
    public class ProjectPriceProjectAppService : MyProjectNameAppService, IProjectPriceProjectAppService
    {
        private IGuidGenerator _guidGenerator;
        private IRepository<Project, Guid> _projectRepository;
        private IRepository<ProjectRltModule, Guid> _projectRltModuleRepository;

        public ProjectPriceProjectAppService(
            IGuidGenerator guidGenerator,
            IRepository<Project, Guid> projectRepository,
            IRepository<ProjectRltModule, Guid> projectRltModuleRepository
            )
        {
            _guidGenerator = guidGenerator;
            _projectRepository = projectRepository;
            _projectRltModuleRepository = projectRltModuleRepository;
        }


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<ProjectDto>> GetList(ProjectGetListInput input)
        {
            var list = _projectRepository
                .WhereIf(!string.IsNullOrEmpty(input.Keywords), x => x.Name.Contains(input.Keywords))
                .OrderByDescending(x => x.CreationTime)
                .ToList();

            return ObjectMapper.Map<List<Project>, List<ProjectDto>>(list);
        }


        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProjectDto> Get(Guid id)
        {
            var project = _projectRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            var projectDto = ObjectMapper.Map<Project, ProjectDto>(project);
            projectDto.ProjectRltModules = projectDto.ProjectRltModules.Where(x => x.ParentId == null).ToList();
            projectDto.ProjectRltModules.OrderBy(x => x.Order).ThenBy(x => x.Module.Order);
            projectDto.ProjectRltModules.ForEach(item =>
            {
                item.Children.OrderBy(x => x.Order).ThenBy(x => x.Module.Order);
            });
            return projectDto;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(ProjectCreateDto input)
        {
            var project = new Project(_guidGenerator.Create())
            {
                Name = input.Name,
                SumPrice = input.SumPrice,
                ParentId = input.ParentId,
                Description = input.Description,
                ProjectRltModules = new List<ProjectRltModule>()
            };

            foreach (var rlt in input.ProjectRltModules)
            {
                project.ProjectRltModules.Add(new ProjectRltModule(_guidGenerator.Create())
                {
                    ProjectId = rlt.ProjectId,
                    ModuleId = rlt.ModuleId,
                    Order = rlt.Order,
                    Price = rlt.Price,
                    Remark = rlt.Remark
                });
            }

            await _projectRepository.InsertAsync(project);
            return true;
        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<Stream> Export(ProjectExportDto input)
        {
            var projectDto = await Get(input.paramter.Id);
            var list = new List<ProjectTemplate>();

            var modules = projectDto.ProjectRltModules;
            modules.ForEach(top =>
            {
                if (top.Children.Count > 0)
                {
                    top.Children.ForEach(sub =>
                    {
                        list.Add(new ProjectTemplate()
                        {
                            Index = modules.IndexOf(top),
                            ModuleName = !string.IsNullOrEmpty(top.Name) ? top.Name : top.Module.Name,
                            SubModuleName = !string.IsNullOrEmpty(sub.Name) ? sub.Name : sub.Module.Name,
                            Content = sub.Module.Content,
                            WorkDays = sub.Module.WorkDays,
                            Progress = sub.Module.Progress,
                            Remark = !string.IsNullOrEmpty(top.Remark) ? top.Remark : top.Module.Remark,
                        });
                    });
                }
                else
                {
                    list.Add(new ProjectTemplate()
                    {
                        Index = modules.IndexOf(top),
                        ModuleName = !string.IsNullOrEmpty(top.Name) ? top.Name : top.Module.Name,
                        SubModuleName = "",
                        Content = top.Module.Content,
                        WorkDays = top.Module.WorkDays,
                        Progress = top.Module.Progress,
                        Remark = !string.IsNullOrEmpty(top.Remark) ? top.Remark : top.Module.Remark,
                    });
                }
            });


            var stream = ExcelHelper.ExcelExportStream(list, input.TemplateKey, input.RowIndex);
            return stream;
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(ProjectUpdateDto input)
        {
            var project = await _projectRepository.GetAsync(input.Id);
            project.Name = input.Name;
            project.ParentId = input.ParentId;
            project.SumPrice = input.SumPrice;
            project.Description = input.Description;

            // 删除之前的关联关系
            await _projectRltModuleRepository.DeleteAsync(x => x.ProjectId == input.Id);

            // 增加新的关联关系
            foreach (var rlt in input.ProjectRltModules)
            {
                var children = new List<ProjectRltModule>();
                foreach (var child in rlt.Children)
                {
                    children.Add(new ProjectRltModule(_guidGenerator.Create())
                    {
                        ProjectId = child.ProjectId,
                        ModuleId = child.ModuleId,
                        Order = child.Order,
                        Price = child.Price,
                        Remark = child.Remark,
                        Name = child.Name
                    });
                }

                project.ProjectRltModules.Add(new ProjectRltModule(_guidGenerator.Create())
                {
                    ProjectId = rlt.ProjectId,
                    ModuleId = rlt.ModuleId,
                    Order = rlt.Order,
                    Price = rlt.Price,
                    Remark = rlt.Remark,
                    Name = rlt.Name,
                    Children = children
                });
            }

            await _projectRepository.UpdateAsync(project);
            return true;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            await _projectRepository.DeleteAsync(id);
            return true;
        }
    }
}
