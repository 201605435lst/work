using SnAbp.StdBasic.Dtos.Model;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.IServices.Model.ModelMVD;
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
    public class StdBasicModelRltMVDPropertyAppSerive : StdBasicAppService, IStdBasicModelRltMVDPropertyAppService
    {
        private readonly IRepository<ModelRltMVDProperty, Guid> _repositoryModelRltMVDProperty;
        private readonly IRepository<MVDProperty, Guid> _repositoryMVDProperty;
        private readonly IGuidGenerator _guidGenerator;

        public StdBasicModelRltMVDPropertyAppSerive(IRepository<ModelRltMVDProperty, Guid> repositoryModelRltMVDProperty, IRepository<MVDProperty, Guid> repositoryMVDProperty,IGuidGenerator guidGenerator
       )
        {
            _repositoryModelRltMVDProperty = repositoryModelRltMVDProperty;
            _guidGenerator = guidGenerator;
            _repositoryMVDProperty = repositoryMVDProperty;
        }

        public async Task<PagedResultDto<ModelRltMVDPropertyDto>> EditList(ModelRltMVDPropertyCreateDto input)
        {
            if (input.ModelId == null || input.ModelId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ModelRltMVDPropertyDto> res = new PagedResultDto<ModelRltMVDPropertyDto>();
            var rltItems = new List<ModelRltMVDProperty>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryModelRltMVDProperty.WithDetails()
                       .Where(x => input.ModelId != null && x.ModelId == input.ModelId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryModelRltMVDProperty.DeleteAsync(m);
                    });

                }
                if (input.list?.Count > 0)
                {
                    List<ModelRltMVDProperty> rltList = new List<ModelRltMVDProperty>();
                    //添加数据
                    input.list.ForEach(m =>
                    {
                        ModelRltMVDProperty model = new ModelRltMVDProperty(_guidGenerator.Create());
                        model.ModelId = input.ModelId;
                        model.Value = m.Value;
                        model.MVDPropertyId = m.MVDPropertyId;
                        _repositoryModelRltMVDProperty.InsertAsync(model);

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
                    var items = new List<ModelRltMVDPropertyDto>();
                    rltItems.ForEach(s =>
                    {
                        ModelRltMVDPropertyDto model = new ModelRltMVDPropertyDto();
                        model.Id = s.Id;
                        model.ModelId = s.ModelId;
                        model.MVDPropertyId = s.MVDPropertyId;
                        model.Value = s.Value;
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

        public Task<PagedResultDto<ModelRltMVDPropertyDto>> GetListByModelId(ModelRltMVDPropertySearchDto input)
        {
            if (input.Ids == null)
                input.Ids = new List<Guid?>();
            var res = new PagedResultDto<ModelRltMVDPropertyDto>();
            var items = new List<ModelRltMVDPropertyDto>();
            if (input.Ids.Count > 0)
            {
                res.TotalCount = input.Ids.Count();
                input.Ids.ForEach(id =>
                {
                    var rltEntity = _repositoryModelRltMVDProperty.WithDetails().Where(x => x.ModelId == input.ModelId && id == x.MVDPropertyId).FirstOrDefault();
                    var rltDto = new ModelRltMVDPropertyDto();
                    if (rltEntity != null)
                    {
                        rltDto = ObjectMapper.Map<ModelRltMVDProperty, ModelRltMVDPropertyDto>(rltEntity);
                        rltDto.Name = rltEntity.MVDPropertyId.HasValue ? rltEntity.MVDProperty.Name : "未定义";
                    }
                    else
                    {
                        var propertyEntity = _repositoryMVDProperty.WithDetails().Where(x => x.Id == id).FirstOrDefault();
                        if (propertyEntity != null)
                        {
                            rltDto.Id = _guidGenerator.Create();
                            rltDto.ModelId = input.ModelId;
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
                var entityList = _repositoryModelRltMVDProperty.WithDetails().Where(x => x.ModelId == input.ModelId).ToList();
                res.TotalCount = entityList.Count();
                // var entities = new List<ModelRltMVDProperty>();
                //entities = input.IsAll ? entityList.ToList() : entityList.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

                entityList.ForEach(x =>
                {
                    var model = new ModelRltMVDPropertyDto();
                    model = ObjectMapper.Map<ModelRltMVDProperty, ModelRltMVDPropertyDto>(x);
                    model.Name = x.MVDPropertyId.HasValue ? x.MVDProperty.Name : "未定义";
                    items.Add(model);
                });
                res.Items = items;
            }
            return Task.FromResult(res);
        }
    }
}
