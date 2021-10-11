using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices.ProductCategory.ProductCategoryMVD;
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
    public class StdBasicProductCategoryRltMVDPropertyAppService : StdBasicAppService, IStdBasicProductCategoryRltMVDPropertyAppService
    {
        private readonly IRepository<ProductCategoryRltMVDProperty, Guid> _repositoryProductCategoryRltMVDProperty;
        private readonly IRepository<MVDProperty, Guid> _repositoryMVDProperty;
        private readonly IGuidGenerator _guidGenerator;

        public StdBasicProductCategoryRltMVDPropertyAppService(IRepository<ProductCategoryRltMVDProperty, Guid> repositoryProductCategoryRltMVDProperty, IRepository<MVDProperty, Guid> repositoryMVDProperty,IGuidGenerator guidGenerator
       )
        {
            _repositoryProductCategoryRltMVDProperty = repositoryProductCategoryRltMVDProperty;
            _guidGenerator = guidGenerator;
            _repositoryMVDProperty = repositoryMVDProperty;
        }

        public async Task<PagedResultDto<ProductCategoryRltMVDPropertyDto>> EditList(ProductCategoryRltMVDPropertyCreateDto input)
        {
            if (input.ProductCategoryId == null || input.ProductCategoryId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProductCategoryRltMVDPropertyDto> res = new PagedResultDto<ProductCategoryRltMVDPropertyDto>();
            var rltItems = new List<ProductCategoryRltMVDProperty>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProductCategoryRltMVDProperty.WithDetails()
                       .Where(x => input.ProductCategoryId != null && x.ProductCategoryId == input.ProductCategoryId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProductCategoryRltMVDProperty.DeleteAsync(m);
                    });

                }
                if (input.list?.Count > 0)
                {
                    List<ProductCategoryRltMVDProperty> rltList = new List<ProductCategoryRltMVDProperty>();
                    //添加数据
                    input.list.ForEach(m =>
                    {
                        ProductCategoryRltMVDProperty ProductCategory = new ProductCategoryRltMVDProperty(_guidGenerator.Create());
                        ProductCategory.ProductCategoryId = input.ProductCategoryId;
                        ProductCategory.Value = m.Value;
                        ProductCategory.MVDPropertyId = m.MVDPropertyId;
                        _repositoryProductCategoryRltMVDProperty.InsertAsync(ProductCategory);

                        rltList.Add(ProductCategory);
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
                    var items = new List<ProductCategoryRltMVDPropertyDto>();
                    rltItems.ForEach(s =>
                    {
                        ProductCategoryRltMVDPropertyDto ProductCategory = new ProductCategoryRltMVDPropertyDto();
                        ProductCategory.Id = s.Id;
                        ProductCategory.ProductCategoryId = s.ProductCategoryId;
                        ProductCategory.MVDPropertyId = s.MVDPropertyId;
                        ProductCategory.Value = s.Value;
                        items.Add(ProductCategory);
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
        public Task<PagedResultDto<ProductCategoryRltMVDPropertyDto>> GetListByProductCategoryId(ProductCategoryRltMVDPropertySearchDto input)
        {
            if (input.Ids == null)
                input.Ids = new List<Guid?>();
            var res = new PagedResultDto<ProductCategoryRltMVDPropertyDto>();
            var items = new List<ProductCategoryRltMVDPropertyDto>();
            if (input.Ids.Count > 0)
            {
                res.TotalCount = input.Ids.Count();
                input.Ids.ForEach(id =>
                {
                    var rltEntity = _repositoryProductCategoryRltMVDProperty.WithDetails().Where(x => x.ProductCategoryId == input.ProductCategoryId && id == x.MVDPropertyId).FirstOrDefault();
                    var rltDto = new ProductCategoryRltMVDPropertyDto();
                    if (rltEntity != null)
                    {
                        rltDto = ObjectMapper.Map<ProductCategoryRltMVDProperty, ProductCategoryRltMVDPropertyDto>(rltEntity);
                        rltDto.Name = rltEntity.MVDPropertyId.HasValue ? rltEntity.MVDProperty.Name : "未定义";
                    }
                    else
                    {
                        var propertyEntity = _repositoryMVDProperty.WithDetails().Where(x => x.Id == id).FirstOrDefault();
                        if (propertyEntity != null)
                        {
                            rltDto.Id = _guidGenerator.Create();
                            rltDto.ProductCategoryId = input.ProductCategoryId;
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
                var entityList = _repositoryProductCategoryRltMVDProperty.WithDetails().Where(x => x.ProductCategoryId == input.ProductCategoryId).ToList();
                res.TotalCount = entityList.Count();
                // var entities = new List<ProductCategoryRltMVDProperty>();
                //entities = input.IsAll ? entityList.ToList() : entityList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                entityList.ForEach(x =>
                {
                    var ProductCategory = new ProductCategoryRltMVDPropertyDto();
                    ProductCategory = ObjectMapper.Map<ProductCategoryRltMVDProperty, ProductCategoryRltMVDPropertyDto>(x);
                    ProductCategory.Name = x.MVDPropertyId.HasValue ? x.MVDProperty.Name : "未定义";
                    items.Add(ProductCategory);
                });
                res.Items = items;
            }
            return Task.FromResult(res);
        }
    }
}
