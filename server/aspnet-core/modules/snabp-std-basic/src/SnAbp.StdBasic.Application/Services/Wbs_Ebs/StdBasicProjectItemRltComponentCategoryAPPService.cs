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
    public class StdBasicProjectItemRltComponentCategoryAppService : StdBasicAppService, IStdBasicProjectItemRltComponentCategoryAPPService
    {
        private readonly IRepository<ProjectItemRltComponentCategory, Guid> _repositoryProjectItemRltComponentCategory;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ComponentCategory, Guid> _repositoryComponentCategory;

        public StdBasicProjectItemRltComponentCategoryAppService(IRepository<ProjectItemRltComponentCategory, Guid> repositoryProjectItemRltComponentCategory,
           IGuidGenerator guidGenerator,
           IRepository<ComponentCategory, Guid> repositoryComponentCategory
       )
        {
            _repositoryProjectItemRltComponentCategory = repositoryProjectItemRltComponentCategory;
            _guidGenerator = guidGenerator;
            _repositoryComponentCategory = repositoryComponentCategory;
        }
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProjectItemRltComponentCategory.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryProjectItemRltComponentCategory.DeleteAsync(id);
            return true;
        }

        /// <summary>
        /// 编辑关联列表（清空关联关系，重新加）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ProjectItemRltComponentCategoryDto>> EditList(ProjectItemRltComponentCategoryCreateDto input)
        {
            if (input.ProjectItemId == null || input.ProjectItemId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProjectItemRltComponentCategoryDto> res = new PagedResultDto<ProjectItemRltComponentCategoryDto>();
            var rltItems = new List<ProjectItemRltComponentCategory>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProjectItemRltComponentCategory.WithDetails()
                       .Where(x => input.ProjectItemId != null && x.ProjectItemId == input.ProjectItemId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProjectItemRltComponentCategory.DeleteAsync(m);
                    });

                }
                if (input.ComponentCategoryIdList?.Count > 0)
                {
                    List<ProjectItemRltComponentCategory> rltList = new List<ProjectItemRltComponentCategory>();
                    //添加数据
                    input.ComponentCategoryIdList.ForEach(m =>
                    {
                        ProjectItemRltComponentCategory model = new ProjectItemRltComponentCategory(_guidGenerator.Create());
                        model.ProjectItemId = input.ProjectItemId;
                        model.ComponentCategoryId = m;
                        _repositoryProjectItemRltComponentCategory.InsertAsync(model);

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

                    var componentList = _repositoryComponentCategory.WithDetails().Where(s => input.ComponentCategoryIdList.Contains(s.Id)).ToList();

                    var items = new List<ProjectItemRltComponentCategoryDto>();
                    rltItems.ForEach(s =>
                    {
                        ProjectItemRltComponentCategoryDto model = new ProjectItemRltComponentCategoryDto();
                        model.Id = s.Id;
                        model.ComponentCategoryId = s.ComponentCategoryId;
                        model.ProjectItemId = s.ProjectItemId;
                        var component = componentList.Find(x => x.Id == s.ComponentCategoryId);
                        if (component != null)
                        {
                            model.Name = component.Name;
                            model.Code = component.Code;
                            model.ExtendCode = component.ExtendCode;
                            model.ExtendName = component.ExtendName;
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


        public async Task<ProjectItemRltComponentCategoryDto> Get(Guid id)
        {
            ProjectItemRltComponentCategoryDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProjectItemRltComponentCategory.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ProjectItemRltComponentCategoryDto();
                result.Id = ent.Id;
                result.ComponentCategoryId = ent.ComponentCategoryId;
                result.ProjectItemId = ent.ProjectItemId;
                if (ent.ComponentCategoryId != null)
                {
                    var component = _repositoryComponentCategory.WithDetails().FirstOrDefault(s => s.Id == ent.ComponentCategoryId);
                    if (component != null)
                    {
                        result.Name = component.Name;
                        result.Code = component.Code;
                        result.ExtendCode = component.ExtendCode;
                        result.ExtendName = component.ExtendName;
                    }
                }

            });
            return result;
        }

        public async Task<PagedResultDto<ProjectItemRltComponentCategoryDto>> GetListByProjectItemId(RltSeachDto input)
        {
            PagedResultDto<ProjectItemRltComponentCategoryDto> res = new PagedResultDto<ProjectItemRltComponentCategoryDto>();
            await Task.Run(() =>
            {
                var allEnt = _repositoryProjectItemRltComponentCategory.WithDetails()
                        .WhereIf(input.Id != null, x => x.ProjectItemId == input.Id).ToList();
                var rltItems = new List<ProjectItemRltComponentCategory>();

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
                    var ids = rltItems.ConvertAll(m => m.ComponentCategoryId);
                    var componentList = _repositoryComponentCategory.WithDetails().Where(s => ids.Contains(s.Id)).ToList();

                    var items = new List<ProjectItemRltComponentCategoryDto>();
                    rltItems.ForEach(s =>
                    {
                        ProjectItemRltComponentCategoryDto model = new ProjectItemRltComponentCategoryDto();
                        model.Id = s.Id;
                        model.ComponentCategoryId = s.ComponentCategoryId;
                        model.ProjectItemId = s.ProjectItemId;
                        var component = componentList.Find(x => x.Id == s.ComponentCategoryId);
                        if (component != null)
                        {
                            model.Name = component.Name;
                            model.Code = component.Code;
                            model.ExtendCode = component.ExtendCode;
                            model.ExtendName = component.ExtendName;
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
