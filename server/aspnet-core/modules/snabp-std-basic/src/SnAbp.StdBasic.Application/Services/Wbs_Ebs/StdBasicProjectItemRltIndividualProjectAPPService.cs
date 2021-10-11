using Microsoft.AspNetCore.Authorization;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    [Authorize]
    public class StdBasicProjectItemRltIndividualProjectAppService : StdBasicAppService, IStdBasicProjectItemRltIndividualProjectAPPService
    {
        private readonly IRepository<ProjectItemRltIndividualProject, Guid> _repositoryProjectItemRltIndividualProject;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ProjectItem, Guid> _repositoryProjectItem;

        public StdBasicProjectItemRltIndividualProjectAppService(IRepository<ProjectItemRltIndividualProject, Guid> repositoryProjectItemRltIndividualProject,
          IGuidGenerator guidGenerator,
          IRepository<ProjectItem, Guid> repositoryProjectItem
      )
        {
            _repositoryProjectItemRltIndividualProject = repositoryProjectItemRltIndividualProject;
            _guidGenerator = guidGenerator;
            _repositoryProjectItem = repositoryProjectItem;
        }
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProjectItemRltIndividualProject.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryProjectItemRltIndividualProject.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ProjectItemRltIndividualProjectDto>> EditList(ProjectItemRltIndividualProjectCreateDto input)
        {
            if (input.IndividualProjectId == null || input.IndividualProjectId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProjectItemRltIndividualProjectDto> res = new PagedResultDto<ProjectItemRltIndividualProjectDto>();
            var rltItems = new List<ProjectItemRltIndividualProject>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProjectItemRltIndividualProject.WithDetails()
                       .Where(x => input.IndividualProjectId != null && x.IndividualProjectId == input.IndividualProjectId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProjectItemRltIndividualProject.DeleteAsync(m);
                    });

                }
                if (input.ProjectItemIdList?.Count > 0)
                {
                    List<ProjectItemRltIndividualProject> rltList = new List<ProjectItemRltIndividualProject>();
                    //添加数据
                    input.ProjectItemIdList.ForEach(m =>
                    {
                        ProjectItemRltIndividualProject model = new ProjectItemRltIndividualProject(_guidGenerator.Create());
                        model.ProjectItemId = m;
                        model.IndividualProjectId = input.IndividualProjectId;
                        _repositoryProjectItemRltIndividualProject.InsertAsync(model);

                        rltList.Add(model);
                    });

                    res.TotalCount = rltList.Count();
                    if (input.IsAll == false)
                    {

                        rltItems = rltList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                    }
                    else
                    {
                        rltItems = rltList;
                    }

                    var componentList = _repositoryProjectItem.WithDetails().Where(s => input.ProjectItemIdList.Contains(s.Id)).ToList();

                    var items = new List<ProjectItemRltIndividualProjectDto>();
                    rltItems.ForEach(s =>
                    {
                        ProjectItemRltIndividualProjectDto model = new ProjectItemRltIndividualProjectDto();
                        model.Id = s.Id;
                        model.IndividualProjectId = s.IndividualProjectId;
                        model.ProjectItemId = s.ProjectItemId;
                        var projectItem = componentList.Find(x => x.Id == s.IndividualProjectId);
                        if (projectItem != null)
                        {
                            model.Name = projectItem.Name;
                            model.Code = projectItem.Code;
                            model.Remark=projectItem.Remark;
                        }
                        items.Add(model);
                    });
                    res.Items = items;
                }
                else
                {
                    res.TotalCount = 0;
                }

            });
            return res;
        }

        public async Task<ProjectItemRltIndividualProjectDto> Get(Guid id)
        {
            ProjectItemRltIndividualProjectDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProjectItemRltIndividualProject.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ProjectItemRltIndividualProjectDto();
                result.Id = ent.Id;
                result.IndividualProjectId = ent.IndividualProjectId;
                result.ProjectItemId = ent.ProjectItemId;
                if (ent.IndividualProjectId != null)
                {
                    var projectItem = _repositoryProjectItem.WithDetails().FirstOrDefault(s => s.Id == ent.IndividualProjectId);
                    if (projectItem != null)
                    {
                        result.Name = projectItem.Name;
                        result.Code = projectItem.Code;
                        result.Remark = projectItem.Remark;
                    }
                }

            });
            return result;
        }

        public async Task<PagedResultDto<ProjectItemRltIndividualProjectDto>> GetListByIndividualProjectId(RltSeachDto input)
        {
            PagedResultDto<ProjectItemRltIndividualProjectDto> res = new PagedResultDto<ProjectItemRltIndividualProjectDto>();
            await Task.Run(() =>
            {
                //单项工程关联工程工项(传入的Id为单项工程Id)
                var allEnt = _repositoryProjectItemRltIndividualProject.WithDetails()
                        .WhereIf(input.Id != null, x => x.IndividualProjectId == input.Id).ToList();
                var rltItems = new List<ProjectItemRltIndividualProject>();

                res.TotalCount = allEnt.Count();
                if (allEnt.Count() > 0)
                {
                    if (input.IsAll == false)
                    {

                        rltItems = allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                    }
                    else
                    {
                        rltItems = allEnt;
                    }
                    var ids = rltItems.ConvertAll(m => m.IndividualProjectId);
                    var componentList = _repositoryProjectItem.WithDetails().Where(s => ids.Contains(s.Id)).ToList();

                    var items = new List<ProjectItemRltIndividualProjectDto>();
                    rltItems.ForEach(s =>
                    {
                        ProjectItemRltIndividualProjectDto model = new ProjectItemRltIndividualProjectDto();
                        model.Id = s.Id;
                        model.IndividualProjectId = s.IndividualProjectId;
                        model.ProjectItemId = s.ProjectItemId;
                        var projectItem = componentList.Find(x => x.Id == s.IndividualProjectId);
                        if (projectItem != null)
                        {
                            model.Name = projectItem.Name;
                            model.Code = projectItem.Code;
                            model.Remark = projectItem.Remark;
                        }
                        items.Add(model);
                    });
                    res.Items = items;
                }
            });
            return res;
        }
    }
}
