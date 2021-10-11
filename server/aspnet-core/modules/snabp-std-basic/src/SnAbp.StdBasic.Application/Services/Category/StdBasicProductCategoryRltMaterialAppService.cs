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
    public class StdBasicProductCategoryRltMaterialAppService : StdBasicAppService, IStdBasicProductCategoryRltMaterialAppService
    {
        private readonly IRepository<ProductCategoryRltMaterial, Guid> _repositoryProductCategoryRltMaterial;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<ComputerCode, Guid> _repositoryComputerCode;

        public StdBasicProductCategoryRltMaterialAppService(IRepository<ProductCategoryRltMaterial, Guid> repositoryProductCategoryRltMaterial,
          IGuidGenerator guidGenerator,
          IRepository<ComputerCode, Guid> repositoryComputerCode
      )
        {
            _repositoryProductCategoryRltMaterial = repositoryProductCategoryRltMaterial;
            _guidGenerator = guidGenerator;
            _repositoryComputerCode = repositoryComputerCode;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProductCategoryRltMaterial.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryProductCategoryRltMaterial.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ProductCategoryRltMaterialDto>> EditList(ProductCategoryRltMaterialCreateDto input)
        {
            if (input.ProductCategoryId == null || input.ProductCategoryId== Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProductCategoryRltMaterialDto> res = new PagedResultDto<ProductCategoryRltMaterialDto>();
            var rltItems = new List<ProductCategoryRltMaterial>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProductCategoryRltMaterial.WithDetails()
                       .Where(x => input.ProductCategoryId != null && x.ProductCategoryId == input.ProductCategoryId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProductCategoryRltMaterial.DeleteAsync(m);
                    });

                }
                if (input.ComputerCodeIdList?.Count > 0)
                {
                    List<ProductCategoryRltMaterial> rltList = new List<ProductCategoryRltMaterial>();
                    //添加数据
                    input.ComputerCodeIdList.ForEach(m =>
                    {
                        ProductCategoryRltMaterial model = new ProductCategoryRltMaterial(_guidGenerator.Create());
                        model.ProductCategoryId = input.ProductCategoryId;
                        model.ComputerCodeId = m;
                        _repositoryProductCategoryRltMaterial.InsertAsync(model);

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

                    var items = new List<ProductCategoryRltMaterialDto>();
                    rltItems.ForEach(s =>
                    {
                        ProductCategoryRltMaterialDto model = new ProductCategoryRltMaterialDto();
                        model.Id = s.Id;
                        model.ProductCategoryId = s.ProductCategoryId;
                        model.ComputerCodeId = s.ComputerCodeId;
                        var computerCode = computerCodeList.Find(x => x.Id == s.ComputerCodeId);
                        if (computerCode != null)
                        {
                            model.Name = computerCode.Name;
                            model.Code = computerCode.Code;
                            model.Unit = computerCode.Unit;
                            model.Weight =(float) computerCode.Weight;
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

        public async Task<ProductCategoryRltMaterialDto> Get(Guid id)
        {
            ProductCategoryRltMaterialDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProductCategoryRltMaterial.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ProductCategoryRltMaterialDto();
                result.Id = ent.Id;
                result.ProductCategoryId = ent.ProductCategoryId;
                result.ComputerCodeId = ent.ComputerCodeId;
                if (ent.ProductCategoryId != null)
                {
                    var computerCode =_repositoryComputerCode.WithDetails().FirstOrDefault(s => s.Id == ent.ComputerCodeId);
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

        public async Task<PagedResultDto<ProductCategoryRltMaterialDto>> GetListByProductCategoryId(RltSeachDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) return null;
            PagedResultDto<ProductCategoryRltMaterialDto> res = new PagedResultDto<ProductCategoryRltMaterialDto>();
            await Task.Run(() =>
            {
                var allEnt = _repositoryProductCategoryRltMaterial.WithDetails()
                        .WhereIf(input.Id != null, x => x.ProductCategoryId == input.Id).ToList();
                var rltItems = new List<ProductCategoryRltMaterial>();

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

                    var items = new List<ProductCategoryRltMaterialDto>();
                    rltItems.ForEach(s =>
                    {
                        ProductCategoryRltMaterialDto model = new ProductCategoryRltMaterialDto();
                        model.Id = s.Id;
                        model.ProductCategoryId = s.ProductCategoryId;
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

