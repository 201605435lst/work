using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices.ComponentCategory.ComponentCategoryMVD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.StdBasic.Services
{
    public class StdBasicComponentCategoryRltMVDPropertyAppService : StdBasicAppService, IStdBasicComponentCategoryRltMVDPropertyAppService
    {
        private readonly IRepository<ComponentCategoryRltMVDProperty, Guid> _repositoryComponentCategoryRltMVDProperty;
        private readonly IRepository<MVDProperty, Guid> _repositoryMVDProperty;
        private readonly IGuidGenerator _guidGenerator;

        public StdBasicComponentCategoryRltMVDPropertyAppService(IRepository<ComponentCategoryRltMVDProperty, Guid> repositoryComponentCategoryRltMVDProperty, IRepository<MVDProperty, Guid> repositoryMVDProperty,IGuidGenerator guidGenerator
       )
        {
            _repositoryComponentCategoryRltMVDProperty = repositoryComponentCategoryRltMVDProperty;
            _guidGenerator = guidGenerator;
            _repositoryMVDProperty = repositoryMVDProperty;
        }

        public async Task<PagedResultDto<ComponentCategoryRltMVDPropertyDto>> EditList(ComponentCategoryRltMVDPropertyCreateDto input)
        {
            if (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ComponentCategoryRltMVDPropertyDto> res = new PagedResultDto<ComponentCategoryRltMVDPropertyDto>();
            var rltItems = new List<ComponentCategoryRltMVDProperty>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryComponentCategoryRltMVDProperty.WithDetails()
                       .Where(x => input.ComponentCategoryId != null && x.ComponentCategoryId == input.ComponentCategoryId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryComponentCategoryRltMVDProperty.DeleteAsync(m);
                    });

                }
                if (input.list?.Count > 0)
                {
                    List<ComponentCategoryRltMVDProperty> rltList = new List<ComponentCategoryRltMVDProperty>();
                    //添加数据
                    input.list.ForEach(m =>
                    {
                        ComponentCategoryRltMVDProperty ComponentCategory = new ComponentCategoryRltMVDProperty(_guidGenerator.Create());
                        ComponentCategory.ComponentCategoryId = input.ComponentCategoryId;
                        ComponentCategory.Value = m.Value;
                        ComponentCategory.MVDPropertyId = m.MVDPropertyId;
                        _repositoryComponentCategoryRltMVDProperty.InsertAsync(ComponentCategory);

                        rltList.Add(ComponentCategory);
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
                    var items = new List<ComponentCategoryRltMVDPropertyDto>();
                    rltItems.ForEach(s =>
                    {
                        ComponentCategoryRltMVDPropertyDto ComponentCategory = new ComponentCategoryRltMVDPropertyDto();
                        ComponentCategory.Id = s.Id;
                        ComponentCategory.ComponentCategoryId = s.ComponentCategoryId;
                        ComponentCategory.MVDPropertyId = s.MVDPropertyId;
                        ComponentCategory.Value = s.Value;
                        items.Add(ComponentCategory);
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

        [HttpPost]
        public Task<PagedResultDto<ComponentCategoryRltMVDPropertyDto>> GetListByComponentCategoryId(ComponentCategoryRltMVDPropertySearchDto input)
        {
            if (input.Ids == null)
                input.Ids = new List<Guid?>();
            var res = new PagedResultDto<ComponentCategoryRltMVDPropertyDto>();
            var items = new List<ComponentCategoryRltMVDPropertyDto>();
            if (input.Ids.Count > 0)
            {
                res.TotalCount = input.Ids.Count();
                input.Ids.ForEach(id =>
                {
                    var rltEntity = _repositoryComponentCategoryRltMVDProperty.WithDetails().Where(x => x.ComponentCategoryId == input.ComponentCategoryId && id == x.MVDPropertyId).FirstOrDefault();
                    var rltDto = new ComponentCategoryRltMVDPropertyDto();
                    if (rltEntity != null)
                    {
                        rltDto = ObjectMapper.Map<ComponentCategoryRltMVDProperty, ComponentCategoryRltMVDPropertyDto>(rltEntity);
                        rltDto.Name = rltEntity.MVDPropertyId.HasValue ? rltEntity.MVDProperty.Name : "未定义";
                    }
                    else
                    {
                        var propertyEntity = _repositoryMVDProperty.WithDetails().Where(x => x.Id == id).FirstOrDefault();
                        if (propertyEntity != null)
                        {
                            rltDto.Id = _guidGenerator.Create();
                            rltDto.ComponentCategoryId = input.ComponentCategoryId;
                            rltDto.Name = propertyEntity.Name;
                            rltDto.Value = null;
                            rltDto.MVDPropertyId = propertyEntity.Id;
                        }
                    }
                    items.Add(rltDto);
                });
                //res.Items = items.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                res.Items = items;
            }
            else
            {
                var entityList = _repositoryComponentCategoryRltMVDProperty.WithDetails().Where(x => x.ComponentCategoryId == input.ComponentCategoryId).ToList();
                res.TotalCount = entityList.Count();
                // var entities = new List<ComponentCategoryRltMVDProperty>();
                //entities = input.IsAll ? entityList.ToList() : entityList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                entityList.ForEach(x =>
                {
                    var ComponentCategory = new ComponentCategoryRltMVDPropertyDto();
                    ComponentCategory = ObjectMapper.Map<ComponentCategoryRltMVDProperty, ComponentCategoryRltMVDPropertyDto>(x);
                    ComponentCategory.Name = x.MVDPropertyId.HasValue ? x.MVDProperty.Name : "未定义";
                    items.Add(ComponentCategory);
                });
                res.Items = items;
            }
            return Task.FromResult(res);
        }
    }
}
