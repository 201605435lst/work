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
    public class StdBasicComponentCategoryRltMaterialAppService : StdBasicAppService, IStdBasicComponentCategoryRltMaterialAppService
    {
        private readonly IRepository<ComponentCategoryRltMaterial, Guid> _repositoryComponentCategoryRltMaterial;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ComputerCode, Guid> _repositoryComputerCode;

        public StdBasicComponentCategoryRltMaterialAppService(IRepository<ComponentCategoryRltMaterial, Guid> repositoryComponentCategoryRltMaterial,
          IGuidGenerator guidGenerator,
          IRepository<ComputerCode, Guid> repositoryComputerCode
      )
        {
            _repositoryComponentCategoryRltMaterial = repositoryComponentCategoryRltMaterial;
            _guidGenerator = guidGenerator;
            _repositoryComputerCode = repositoryComputerCode;
        }
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryComponentCategoryRltMaterial.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryComponentCategoryRltMaterial.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ComponentCategoryRltMaterialDto>> EditList(ComponentCategoryRltMaterialCreateDto input)
        {
            if (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ComponentCategoryRltMaterialDto> res = new PagedResultDto<ComponentCategoryRltMaterialDto>();
            var rltItems = new List<ComponentCategoryRltMaterial>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryComponentCategoryRltMaterial.WithDetails()
                       .Where(x => input.ComponentCategoryId != null && x.ComponentCategoryId == input.ComponentCategoryId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryComponentCategoryRltMaterial.DeleteAsync(m);
                    });

                }
                if (input.ComputerCodeIdList?.Count > 0)
                {
                    List<ComponentCategoryRltMaterial> rltList = new List<ComponentCategoryRltMaterial>();
                    //添加数据
                    input.ComputerCodeIdList.ForEach(m =>
                    {
                        ComponentCategoryRltMaterial model = new ComponentCategoryRltMaterial(_guidGenerator.Create());
                        model.ComponentCategoryId = input.ComponentCategoryId;
                        model.ComputerCodeId = m;
                        _repositoryComponentCategoryRltMaterial.InsertAsync(model);

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

                    var computerCodeList = _repositoryComputerCode.WithDetails().Where(s => input.ComputerCodeIdList.Contains(s.Id)).ToList();

                    var items = new List<ComponentCategoryRltMaterialDto>();
                    rltItems.ForEach(s =>
                    {
                        ComponentCategoryRltMaterialDto model = new ComponentCategoryRltMaterialDto();
                        model.Id = s.Id;
                        model.ComponentCategoryId = s.ComponentCategoryId;
                        model.ComputerCodeId = s.ComputerCodeId;
                        var computerCode = computerCodeList.Find(x => x.Id == s.ComputerCodeId);
                        if (computerCode != null)
                        {
                            model.Name = computerCode.Name;
                            model.Code = computerCode.Code;
                            model.Unit = computerCode.Unit;
                            model.Weight = (float)computerCode.Weight;
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

        public async Task<ComponentCategoryRltMaterialDto> Get(Guid id)
        {
            ComponentCategoryRltMaterialDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryComponentCategoryRltMaterial.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ComponentCategoryRltMaterialDto();
                result.Id = ent.Id;
                result.ComponentCategoryId = ent.ComponentCategoryId;
                result.ComputerCodeId = ent.ComputerCodeId;
                if (ent.ComponentCategoryId != null)
                {
                    var computerCode = _repositoryComputerCode.WithDetails().FirstOrDefault(s => s.Id == ent.ComputerCodeId);
                    if (computerCode != null)
                    {
                        result.Name = computerCode.Name;
                        result.Code = computerCode.Code;
                        result.Unit = computerCode.Unit;
                        result.Weight = (float)computerCode.Weight;
                    }
                }

            });
            return result;
        }

        public async Task<PagedResultDto<ComponentCategoryRltMaterialDto>> GetListByComponentCategoryId(RltSeachDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) return null;
            PagedResultDto<ComponentCategoryRltMaterialDto> res = new PagedResultDto<ComponentCategoryRltMaterialDto>();
            await Task.Run(() =>
            {
                var allEnt = _repositoryComponentCategoryRltMaterial.WithDetails()
                        .WhereIf(input.Id != null, x => x.ComponentCategoryId == input.Id).ToList();
                var rltItems = new List<ComponentCategoryRltMaterial>();

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
                    var ids = rltItems.ConvertAll(m => m.ComputerCodeId);
                    var computerCodeList = _repositoryComputerCode.WithDetails().Where(s => ids.Contains(s.Id)).ToList();

                    var items = new List<ComponentCategoryRltMaterialDto>();
                    rltItems.ForEach(s =>
                    {
                        ComponentCategoryRltMaterialDto model = new ComponentCategoryRltMaterialDto();
                        model.Id = s.Id;
                        model.ComponentCategoryId = s.ComponentCategoryId;
                        model.ComputerCodeId = s.ComputerCodeId;
                        var computerCode = computerCodeList.Find(x => x.Id == s.ComputerCodeId);
                        if (computerCode != null)
                        {
                            model.Name = computerCode.Name;
                            model.Code = computerCode.Code;
                            model.Unit = computerCode.Unit;
                            model.Weight = (float)computerCode.Weight;
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
