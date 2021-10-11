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
    public class StdBasicProjectItemRltProductCategoryAppService : StdBasicAppService, IStdBasicProjectItemRltProductCategoryAPPService
    {
        private readonly IRepository<ProjectItemRltProductCategory, Guid> _repositoryProjectItemRltProductCategory;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ProductCategory, Guid> _repositoryProductCategory;

        public StdBasicProjectItemRltProductCategoryAppService(IRepository<ProjectItemRltProductCategory, Guid> repositoryProjectItemRltProductCategory,
          IGuidGenerator guidGenerator,
          IRepository<ProductCategory, Guid> repositoryProductCategory
      )
        {
            _repositoryProjectItemRltProductCategory = repositoryProjectItemRltProductCategory;
            _guidGenerator = guidGenerator;
            _repositoryProductCategory = repositoryProductCategory;
        }
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProjectItemRltProductCategory.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryProjectItemRltProductCategory.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ProjectItemRltProductCategoryDto>> EditList(ProjectItemRltProductCategoryCreateDto input)
        {
            if (input.ProjectItemId == null || input.ProjectItemId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProjectItemRltProductCategoryDto> res = new PagedResultDto<ProjectItemRltProductCategoryDto>();
            var rltItems = new List<ProjectItemRltProductCategory>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProjectItemRltProductCategory.WithDetails()
                       .Where(x => input.ProjectItemId != null && x.ProjectItemId == input.ProjectItemId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProjectItemRltProductCategory.DeleteAsync(m);
                    });

                }
                if (input.ProductCategoryIdList?.Count > 0)
                {
                    List<ProjectItemRltProductCategory> rltList = new List<ProjectItemRltProductCategory>();
                    //添加数据
                    input.ProductCategoryIdList.ForEach(m =>
                    {
                        ProjectItemRltProductCategory model = new ProjectItemRltProductCategory(_guidGenerator.Create());
                        model.ProjectItemId = input.ProjectItemId;
                        model.ProductCategoryId = m;
                        _repositoryProjectItemRltProductCategory.InsertAsync(model);

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

                    var productList = _repositoryProductCategory.WithDetails().Where(s => input.ProductCategoryIdList.Contains(s.Id)).ToList();

                    var items = new List<ProjectItemRltProductCategoryDto>();
                    rltItems.ForEach(s =>
                    {
                        ProjectItemRltProductCategoryDto model = new ProjectItemRltProductCategoryDto();
                        model.Id = s.Id;
                        model.ProductCategoryId = s.ProductCategoryId;
                        model.ProjectItemId = s.ProjectItemId;
                        var product = productList.Find(x => x.Id == s.ProductCategoryId);
                        if (product != null)
                        {
                            model.Name = product.Name;
                            model.Code = product.Code;
                            model.ExtendCode = product.ExtendCode;
                            model.ExtendName = product.ExtendName;
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

        public async Task<ProjectItemRltProductCategoryDto> Get(Guid id)
        {
            ProjectItemRltProductCategoryDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProjectItemRltProductCategory.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ProjectItemRltProductCategoryDto();
                result.Id = ent.Id;
                result.ProductCategoryId = ent.ProductCategoryId;
                result.ProjectItemId = ent.ProjectItemId;
                if (ent.ProductCategoryId != null)
                {
                    var product = _repositoryProductCategory.WithDetails().FirstOrDefault(s => s.Id == ent.ProductCategoryId);
                    if (product != null)
                    {
                        result.Name = product.Name;
                        result.Code = product.Code;
                        result.ExtendCode = product.ExtendCode;
                        result.ExtendName = product.ExtendName;
                    }
                }

            });
            return result;
        }

        public async Task<PagedResultDto<ProjectItemRltProductCategoryDto>> GetListByProjectItemId(RltSeachDto input)
        {
            PagedResultDto<ProjectItemRltProductCategoryDto> res = new PagedResultDto<ProjectItemRltProductCategoryDto>();
            await Task.Run(() =>
            {
                var allEnt = _repositoryProjectItemRltProductCategory.WithDetails()
                        .WhereIf(input.Id != null, x => x.ProjectItemId == input.Id).ToList();
                var rltItems = new List<ProjectItemRltProductCategory>();

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
                    var ids = rltItems.ConvertAll(m => m.ProductCategoryId);
                    var productList = _repositoryProductCategory.WithDetails().Where(s => ids.Contains(s.Id)).ToList();

                    var items = new List<ProjectItemRltProductCategoryDto>();
                    rltItems.ForEach(s =>
                    {
                        ProjectItemRltProductCategoryDto model = new ProjectItemRltProductCategoryDto();
                        model.Id = s.Id;
                        model.ProductCategoryId = s.ProductCategoryId;
                        model.ProjectItemId = s.ProjectItemId;
                        var product = productList.Find(x => x.Id == s.ProductCategoryId);
                        if (product != null)
                        {
                            model.Name = product.Name;
                            model.Code = product.Code;
                            model.ExtendCode = product.ExtendCode;
                            model.ExtendName = product.ExtendName;
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
