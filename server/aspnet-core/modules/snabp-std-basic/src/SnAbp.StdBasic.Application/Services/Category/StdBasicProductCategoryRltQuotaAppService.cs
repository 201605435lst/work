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
    public class StdBasicProductCategoryRltQuotaAppService : StdBasicAppService, IStdBasicProductCategoryRltQuotaAppService
    {
        private readonly IRepository<ProductCategoryRltQuota, Guid> _repositoryProductCategoryRltQuota;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Quota, Guid> _repositoryQuota;
        private readonly IRepository<QuotaCategory, Guid> _repositoryQuotaCategory;
        public StdBasicProductCategoryRltQuotaAppService(IRepository<ProductCategoryRltQuota, Guid> repositoryProductCategoryRltQuota,
          IGuidGenerator guidGenerator,
          IRepository<Quota, Guid> repositoryQuota,
          IRepository<QuotaCategory, Guid> repositoryQuotaCategory
      )
        {
            _repositoryProductCategoryRltQuota = repositoryProductCategoryRltQuota;
            _guidGenerator = guidGenerator;
            _repositoryQuota = repositoryQuota;
            _repositoryQuotaCategory = repositoryQuotaCategory;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryProductCategoryRltQuota.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryProductCategoryRltQuota.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ProductCategoryRltQuotaDto>> EditList(ProductCategoryRltQuotaCreateDto input)
        {
            if (input.ProductionCategoryId == null || input.ProductionCategoryId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ProductCategoryRltQuotaDto> res = new PagedResultDto<ProductCategoryRltQuotaDto>();
            var rltItems = new List<ProductCategoryRltQuota>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryProductCategoryRltQuota.WithDetails()
                       .Where(x => input.ProductionCategoryId != null && x.ProductionCategoryId == input.ProductionCategoryId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryProductCategoryRltQuota.DeleteAsync(m);
                    });

                }
                if (input.QuotaIdList?.Count > 0)
                {
                    List<ProductCategoryRltQuota> rltList = new List<ProductCategoryRltQuota>();
                    //添加数据
                    input.QuotaIdList.ForEach(m =>
                    {
                        ProductCategoryRltQuota model = new ProductCategoryRltQuota(_guidGenerator.Create());
                        model.ProductionCategoryId = input.ProductionCategoryId;
                        model.QuotaId = m;
                        _repositoryProductCategoryRltQuota.InsertAsync(model);

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

                    var quotaList = _repositoryQuota.Where(s => input.QuotaIdList.Contains(s.Id)).ToList();

                    var quotaCategoryList = new List<QuotaCategory>();
                    var quotaCategoryIds = quotaList.ConvertAll(a => a.QuotaCategoryId);
                    quotaCategoryList = _repositoryQuotaCategory.Where(a => quotaCategoryIds.Contains(a.Id)).ToList();
                    var items = new List<ProductCategoryRltQuotaDto>();
                    rltItems.ForEach(s =>
                    {
                        ProductCategoryRltQuotaDto model = new ProductCategoryRltQuotaDto();
                        model.Id = s.Id;
                        model.ProductionCategoryId = s.ProductionCategoryId;
                        model.QuotaId = s.QuotaId;
                        var quota = quotaList.Find(x => x.Id == s.QuotaId);
                        if (quota != null)
                        {
                            var quotaCategory = quotaCategoryList.Find(a => a.Id == quota.QuotaCategoryId);
                            model.Name = quota.Name;
                            model.Code = quota.Code;
                            model.Unit = quota.Unit;
                            model.Weight = (float)quota.Weight;
                            model.Content = quotaCategory.Content;
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

        public async Task<ProductCategoryRltQuotaDto> Get(Guid id)
        {
            ProductCategoryRltQuotaDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryProductCategoryRltQuota.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ProductCategoryRltQuotaDto();
                result.Id = ent.Id;
                result.ProductionCategoryId = ent.ProductionCategoryId;
                result.QuotaId = ent.QuotaId;
                if (ent.ProductionCategoryId != null)
                {
                    var quota = _repositoryQuota.WithDetails().FirstOrDefault(s => s.Id == ent.QuotaId);
                    if (quota != null)
                    {
                        var quotaCategory = _repositoryQuotaCategory.FirstOrDefault(a => a.Id == quota.QuotaCategoryId);
                        result.Name = quota.Name;
                        result.Code = quota.Code;
                        result.Unit = quota.Unit;
                        result.Weight = (float)quota.Weight;
                        result.Content = quotaCategory.Content;
                    }
                }

            });
            return result;
        }

        public async Task<PagedResultDto<ProductCategoryRltQuotaDto>> GetListByProductCategoryId(RltSeachDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) return null;
            PagedResultDto<ProductCategoryRltQuotaDto> res = new PagedResultDto<ProductCategoryRltQuotaDto>();
           
            await Task.Run(() =>
            {
                var allEnt = _repositoryProductCategoryRltQuota.WithDetails()
                        .WhereIf(input.Id != null, x => x.ProductionCategoryId == input.Id).ToList();
                var rltItems = new List<ProductCategoryRltQuota>();

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
                    var ids = rltItems.ConvertAll(m => m.QuotaId);
                    var quotaList = _repositoryQuota.WithDetails().Where(s => ids.Contains(s.Id)).ToList();
                    var quotaCategoryIds = quotaList.ConvertAll(a => a.QuotaCategoryId);
                    var quotaCategoryList = new List<QuotaCategory>();
                    quotaCategoryList = _repositoryQuotaCategory.Where(a => quotaCategoryIds.Contains(a.Id)).ToList();
                    var items = new List<ProductCategoryRltQuotaDto>();
                    rltItems.ForEach(s =>
                    {
                        ProductCategoryRltQuotaDto model = new ProductCategoryRltQuotaDto();
                        model.Id = s.Id;
                        model.ProductionCategoryId = s.ProductionCategoryId;
                        model.QuotaId = s.QuotaId;
                        var quota = quotaList.Find(x => x.Id == s.QuotaId);
                        if (quota != null)
                        {
                            var quotaCategory = quotaCategoryList.Find(a => a.Id == quota.QuotaCategoryId);
                            model.Name = quota.Name;
                            model.Code = quota.Code;
                            model.Unit = quota.Unit;
                            model.Weight = (float)quota.Weight;
                            model.Content = quotaCategory.Content;
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
