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
    public class StdBasicComponentCategoryRltQuotaAppService : StdBasicAppService, IStdBasicComponentCategoryRltQuotaAppService
    {
        private readonly IRepository<ComponentCategoryRltQuota, Guid> _repositoryComponentCategoryRltQuota;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<Quota, Guid> _repositoryQuota;
        private readonly IRepository<QuotaCategory, Guid> _repositoryQuotaCategory;

        public StdBasicComponentCategoryRltQuotaAppService(IRepository<ComponentCategoryRltQuota, Guid> repositoryComponentCategoryRltQuota,
          IGuidGenerator guidGenerator,
          IRepository<Quota, Guid> repositoryQuota,
          IRepository<QuotaCategory, Guid> repositoryQuotaCategory
      )
        {
            _repositoryComponentCategoryRltQuota = repositoryComponentCategoryRltQuota;
            _guidGenerator = guidGenerator;
            _repositoryQuota = repositoryQuota;
            _repositoryQuotaCategory = repositoryQuotaCategory;
        }
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var ent = _repositoryComponentCategoryRltQuota.WithDetails().FirstOrDefault(s => s.Id == id);
            if (ent == null) throw new UserFriendlyException("此关联关系不存在");
            await _repositoryComponentCategoryRltQuota.DeleteAsync(id);
            return true;
        }

        public async Task<PagedResultDto<ComponentCategoryRltQuotaDto>> EditList(ComponentCategoryRltQuotaCreateDto input)
        {
            if (input.ComponentCategoryId == null || input.ComponentCategoryId == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            PagedResultDto<ComponentCategoryRltQuotaDto> res = new PagedResultDto<ComponentCategoryRltQuotaDto>();
            var rltItems = new List<ComponentCategoryRltQuota>();

            await Task.Run(() =>
            {
                var allEnt = _repositoryComponentCategoryRltQuota.WithDetails()
                       .Where(x => input.ComponentCategoryId != null && x.ComponentCategoryId == input.ComponentCategoryId).ToList();
                if (allEnt?.Count > 0)
                {
                    //删除数据
                    allEnt.ForEach(m =>
                    {
                        _repositoryComponentCategoryRltQuota.DeleteAsync(m);
                    });

                }
                if (input.QuotaIdList?.Count > 0)
                {
                    List<ComponentCategoryRltQuota> rltList = new List<ComponentCategoryRltQuota>();
                    //添加数据
                    input.QuotaIdList.ForEach(m =>
                    {
                        ComponentCategoryRltQuota model = new ComponentCategoryRltQuota(_guidGenerator.Create());
                        model.ComponentCategoryId = input.ComponentCategoryId;
                        model.QuotaId = m;
                        _repositoryComponentCategoryRltQuota.InsertAsync(model);

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
                    var quotaCategoryIds = quotaList.ConvertAll(a=>a.QuotaCategoryId);
                    quotaCategoryList = _repositoryQuotaCategory.Where(a => quotaCategoryIds.Contains(a.Id)).ToList();
                    var items = new List<ComponentCategoryRltQuotaDto>();
                    rltItems.ForEach(s =>
                    {
                        ComponentCategoryRltQuotaDto model = new ComponentCategoryRltQuotaDto();
                        model.Id = s.Id;
                        model.ComponentCategoryId = s.ComponentCategoryId;
                        model.QuotaId = s.QuotaId;
                        var quota = quotaList.Find(x => x.Id == s.QuotaId);
                        if (quota != null)
                        {
                           var quotaCategory = quotaCategoryList.Find(a=>a.Id==quota.QuotaCategoryId);
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

        public async Task<ComponentCategoryRltQuotaDto> Get(Guid id)
        {
            ComponentCategoryRltQuotaDto result = null;
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            await Task.Run(() =>
            {
                var ent = _repositoryComponentCategoryRltQuota.WithDetails().FirstOrDefault(s => s.Id == id);
                if (ent == null) throw new UserFriendlyException("此关联关系不存在");
                result = new ComponentCategoryRltQuotaDto();
                result.Id = ent.Id;
                result.ComponentCategoryId = ent.ComponentCategoryId;
                result.QuotaId = ent.QuotaId;
                if (ent.ComponentCategoryId != null)
                {
                    var quota = _repositoryQuota.WithDetails().FirstOrDefault(s => s.Id == ent.QuotaId);
                    if (quota != null)
                    {
                        var quotaCategory =_repositoryQuotaCategory.FirstOrDefault(a => a.Id == quota.QuotaCategoryId);
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

        public async Task<PagedResultDto<ComponentCategoryRltQuotaDto>> GetListByComponentCategoryId(RltSeachDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty) return null;
            PagedResultDto<ComponentCategoryRltQuotaDto> res = new PagedResultDto<ComponentCategoryRltQuotaDto>();
            await Task.Run(() =>
            {
                var allEnt = _repositoryComponentCategoryRltQuota.WithDetails()
                        .WhereIf(input.Id != null, x => x.ComponentCategoryId == input.Id).ToList();
                var rltItems = new List<ComponentCategoryRltQuota>();

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
                    var items = new List<ComponentCategoryRltQuotaDto>();
                    rltItems.ForEach(s =>
                    {
                        ComponentCategoryRltQuotaDto model = new ComponentCategoryRltQuotaDto();
                        model.Id = s.Id;
                        model.ComponentCategoryId = s.ComponentCategoryId;
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
