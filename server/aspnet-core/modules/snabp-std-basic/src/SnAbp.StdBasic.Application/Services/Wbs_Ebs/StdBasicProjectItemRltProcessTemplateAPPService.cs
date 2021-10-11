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
    public class StdBasicProjectItemRltProcessTemplateAppService : StdBasicAppService, IStdBasicProjectItemRltProcessTemplateAPPService
    {
        private readonly IRepository<ProjectItemRltProcessTemplate, Guid> _repositoryProjectItemRltProcessTemplate;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ProjectItem, Guid> _repositoryProjectItem;
        private readonly IRepository<IndividualProject, Guid> _repositoryIndividualProject;
        public StdBasicProjectItemRltProcessTemplateAppService(IRepository<ProjectItemRltProcessTemplate, Guid> repositoryProjectItemRltProcessTemplate,
         IGuidGenerator guidGenerator,
         IRepository<ProjectItem, Guid> repositoryProjectItem,
           IRepository<IndividualProject, Guid> repositoryIndividualProject
     )
        {
            _repositoryProjectItemRltProcessTemplate = repositoryProjectItemRltProcessTemplate;
            _guidGenerator = guidGenerator;
            _repositoryProjectItem = repositoryProjectItem;
            _repositoryIndividualProject = repositoryIndividualProject;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProjectItemRltProcessTemplate.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryProjectItemRltProcessTemplate.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ProjectItemRltProcessTemplateDto>> EditList(ProjectItemRltProcessTemplateCreateDto input)
        {
            if (input.ProcessTemplateId == null || input.ProcessTemplateId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProjectItemRltProcessTemplateDto> res = new PagedResultDto<ProjectItemRltProcessTemplateDto>();
            var rltItems = new List<ProjectItemRltProcessTemplate>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProjectItemRltProcessTemplate.WithDetails()
                      .Where(x => input.ProcessTemplateId != null && x.ProcessTemplateId == input.ProcessTemplateId).ToList();
                if (allEnt?.Count > 0)
                {
                    //var ids = allEnt.ConvertAll(m => m.ProjectItemId);
                    //var deleteIds = new List<Guid>();
                    //if (input.Type == 0)//单项工程
                    //{
                    //    var individuals = _repositoryIndividualProject.Where(x => ids.Contains(x.Id)).ToList();
                    //    if (individuals?.Count() > 0)
                    //    {
                    //        deleteIds = individuals.ConvertAll(m => m.Id);
                    //    }

                    //}
                    //else
                    //{
                    //    var projectItems = _repositoryProjectItem.Where(x => ids.Contains(x.Id)).ToList();
                    //    if (projectItems?.Count() > 0)
                    //    {
                    //        deleteIds = projectItems.ConvertAll(m => m.Id);
                    //    }

                    //}

                    //allEnt = allEnt.FindAll(x => deleteIds.Contains(x.ProjectItemId));
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProjectItemRltProcessTemplate.DeleteAsync(m);
                    });

                }
                if (input.ProjectItemIdList?.Count > 0)
                {
                    List<ProjectItemRltProcessTemplate> rltList = new List<ProjectItemRltProcessTemplate>();
                    //添加数据
                    input.ProjectItemIdList.ForEach(m =>
                    {
                        ProjectItemRltProcessTemplate model = new ProjectItemRltProcessTemplate(_guidGenerator.Create());
                        model.ProjectItemId = m;
                        model.ProcessTemplateId = input.ProcessTemplateId;
                        _repositoryProjectItemRltProcessTemplate.InsertAsync(model);

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
                    var items = new List<ProjectItemRltProcessTemplateDto>();
                    var ids = rltItems.ConvertAll(m => m.ProjectItemId);

                    var projectItemList = _repositoryProjectItem.WithDetails().Where(s => ids.Contains(s.Id)).ToList();
                    if (projectItemList?.Count > 0)
                    {
                        projectItemList.ForEach(s =>
                        {
                            var rltModel = rltItems.Find(x => x.ProjectItemId == s.Id);
                            ProjectItemRltProcessTemplateDto model = new ProjectItemRltProcessTemplateDto();
                            model.Id = rltModel.Id;
                            model.ProcessTemplateId = rltModel.ProcessTemplateId;
                            model.ProjectItemId = rltModel.ProjectItemId;
                            model.Name = s.Name;
                            model.Code = s.Code;
                            model.Remark = s.Remark;
                            model.Type = 1;
                            items.Add(model);


                        });
                    }
                    var individualList = _repositoryIndividualProject.WithDetails().Where(s => ids.Contains(s.Id)).ToList();
                    if (individualList?.Count > 0)
                    {
                        individualList.ForEach(s =>
                        {
                            var rltModel = rltItems.Find(x => x.ProjectItemId == s.Id);
                            ProjectItemRltProcessTemplateDto model = new ProjectItemRltProcessTemplateDto();
                            model.Id = rltModel.Id;
                            model.ProcessTemplateId = rltModel.ProcessTemplateId;
                            model.ProjectItemId = rltModel.ProjectItemId;

                            model.Name = s.Name;
                            model.Code = s.Code;
                            model.Remark = s.Remark;
                            model.SpecialtyId = s.SpecialtyId;
                            model.SpecialtyName = s.Specialty?.Name;
                            model.Type = 0;
                            items.Add(model);
                        });
                    }
                    res.Items = items;
                }
                else
                {
                    res.TotalCount = 0;
                }

            });
            return res;
        }

        public async Task<ProjectItemRltProcessTemplateDto> Get(Guid id)
        {
            ProjectItemRltProcessTemplateDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProjectItemRltProcessTemplate.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ProjectItemRltProcessTemplateDto();
                result.Id = ent.Id;
                result.ProcessTemplateId = ent.ProcessTemplateId;
                result.ProjectItemId = ent.ProjectItemId;
                if (ent.ProjectItemId != null)
                {

                    var projectItem = _repositoryProjectItem.WithDetails().FirstOrDefault(s => s.Id == ent.ProjectItemId);
                    if (projectItem != null)
                    {
                        result.Name = projectItem.Name;
                        result.Code = projectItem.Code;
                        result.Remark = projectItem.Remark;
                        result.Type = 1;
                    }
                    else
                    {
                        var individual = _repositoryIndividualProject.WithDetails().FirstOrDefault(s => s.Id == ent.ProjectItemId);
                        if (individual != null)
                        {
                            result.Name = individual.Name;
                            result.Code = individual.Code;
                            result.SpecialtyId = individual.SpecialtyId;
                            result.Remark = individual.Remark;
                            result.Type = 0;
                        }
                    }
                }

            });
            return result;
        }

        public async Task<PagedResultDto<ProjectItemRltProcessTemplateDto>> GetListByProcessTemplateId(ProjectItemRltProcessTemplateSeachDto input)
        {
            PagedResultDto<ProjectItemRltProcessTemplateDto> res = new PagedResultDto<ProjectItemRltProcessTemplateDto>();
            await Task.Run(() =>
            {
                //var allEnt = _repositoryProjectItemRltProcessTemplate.WithDetails()
                //        .WhereIf(input.Id != null, x => x.ProjectItemId == input.Id).ToList();
                var allEnt = _repositoryProjectItemRltProcessTemplate.WithDetails()
                  .WhereIf(input.Id != null, x => x.ProcessTemplateId == input.Id).ToList();
                var rltItems = new List<ProjectItemRltProcessTemplateDto>();
                var resItems = new List<ProjectItemRltProcessTemplateDto>();
                res.TotalCount = allEnt.Count();
                if (allEnt.Count() > 0)
                
                {
                    if (input.IsAll == false)
                    {

                        rltItems = ObjectMapper.Map < List<ProjectItemRltProcessTemplate>, List<ProjectItemRltProcessTemplateDto> > (allEnt.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());

                    }
                    else
                    {
                        rltItems = ObjectMapper.Map<List<ProjectItemRltProcessTemplate>, List<ProjectItemRltProcessTemplateDto>>(allEnt);
                    }
                   
                    if (rltItems?.Count > 0)
                    {
                        var ids = rltItems.ConvertAll(m => m.ProjectItemId);
                        var items = new List<ProjectItemRltProcessTemplateDto>();

                        var projectItemList = _repositoryProjectItem.WithDetails().Where(s => ids.Contains(s.Id)).ToList();
                        var individualList = _repositoryIndividualProject.WithDetails().Where(s => ids.Contains(s.Id)).ToList();
                        rltItems.ForEach(x=>
                        {
                            var model = projectItemList.Find(m=>m.Id==x.ProjectItemId);
                            if(model!=null)
                            {
                                x.Name = model.Name;
                                x.Code = model.Code;
                                x.Remark = model.Remark;
                                x.Type = 1;
                                resItems.Add(x);
                            }
                            else
                            {
                                var inisModel = individualList.Find(m => m.Id == x.ProjectItemId);
                                if (inisModel != null)
                                {
                                    x.Name = inisModel.Name;
                                    x.Code = inisModel.Code;
                                    x.Remark = inisModel.Remark;
                                    x.SpecialtyId = inisModel.SpecialtyId;
                                    x.SpecialtyName = inisModel.Specialty?.Name;
                                    x.Type = 0;
                                    resItems.Add(x);
                                }
                                else
                                {
                                    _repositoryProjectItemRltProcessTemplate.DeleteAsync(x.Id);
                                }
                            }

                        });
                    }
                }
                res.Items = resItems;
            });
            return res;
        }
    }
}
