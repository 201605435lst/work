using Microsoft.AspNetCore.Mvc;
using SnAbp.Construction.Dtos;
using SnAbp.Construction.Entities;
using SnAbp.Construction.IServices.Daily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

/************************************************************************************
*命名空间：SnAbp.Construction.Services.Daily
*文件名：ConstructionDailyTemplateAppService
*创建人： liushengtao
*创建时间：2021/7/30 15:18:32
*描述：
*
***********************************************************************/
namespace SnAbp.Construction.Services
{
    public class ConstructionDailyTemplateAppService : ConstructionAppService, IConstructionDailyTemplateAppService
    {
        private readonly IRepository<DailyTemplate, Guid> _dailyTemplateRepository;
        private readonly IGuidGenerator _guidGenerator;

        public ConstructionDailyTemplateAppService(
            IRepository<DailyTemplate, Guid> dailyTemplateRepository,
            IGuidGenerator guidGenerator
        )
        {
            _dailyTemplateRepository = dailyTemplateRepository;
            _guidGenerator = guidGenerator;
        }
        public async Task<bool> Create(DailyTemplateSimpleDto input)
        {
            if (input.Name == null) throw new UserFriendlyException("模板名称不能为空");
            await CheckSameName(input.Name, null);
            var query = _dailyTemplateRepository.FirstOrDefault(x => x.IsDefault);
            if (input.IsDefault)
            {
                if (query != null)
                {
                    query.IsDefault = false;
                    await _dailyTemplateRepository.UpdateAsync(query);
                }
            }
            var dailys = new DailyTemplate(_guidGenerator.Create())
            {
                Name = input.Name,
                Description = input.Description,
                IsDefault = query == null ? true : input.IsDefault,
                Remark = input.Remark,
            };

            await _dailyTemplateRepository.InsertAsync(dailys);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的Id");
            await _dailyTemplateRepository.DeleteAsync(id);

            return true;
        }

        public Task<DailyTemplateDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请确定要查询的数据");

            var daily = _dailyTemplateRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (daily == null) throw new UserFriendlyException("当前数据不存在");

            return Task.FromResult(ObjectMapper.Map<DailyTemplate, DailyTemplateDto>(daily));
        }

        public Task<PagedResultDto<DailyTemplateDto>> GetList(DailyTemplateSearchDto input)
        {
            var dailyTemplates = _dailyTemplateRepository.WithDetails()
                .Where(x => x.CreatorId == CurrentUser.Id)
                 .WhereIf(!string.IsNullOrWhiteSpace(input.KeyWords), x => x.Name.Contains(input.KeyWords) || x.Description.Contains(input.KeyWords));

            var result = new PagedResultDto<DailyTemplateDto>();
            dailyTemplates = dailyTemplates.OrderBy(x => x.CreationTime);
            result.TotalCount = dailyTemplates.Count();
            if (input.IsAll)
            {
                result.Items = ObjectMapper.Map<List<DailyTemplate>, List<DailyTemplateDto>>(dailyTemplates.ToList());
            }
            else
            {
                result.Items = ObjectMapper.Map<List<DailyTemplate>, List<DailyTemplateDto>>(dailyTemplates.Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            }
            return Task.FromResult(result);
        }
        [HttpGet]
        public async Task<bool> SetDefault(Guid id)
        {
            if (id == Guid.Empty || id == null) throw new UserFriendlyException("当前数据错误，请刷新页面重新尝试");
            var query = _dailyTemplateRepository.FirstOrDefault(x => x.IsDefault);
            if (query != null)
            {
                query.IsDefault = false;
                await _dailyTemplateRepository.UpdateAsync(query);
            }
            var daily = _dailyTemplateRepository.FirstOrDefault(x => x.Id == id);
            if (daily == null) throw new UserFriendlyException("当前数据不存在，请刷新页面重新尝试");
            daily.IsDefault = true;
            await _dailyTemplateRepository.UpdateAsync(daily);
            return true;
        }

        public async Task<bool> Update(DailyTemplateSimpleDto input)
        {
            if (input.Name == null) throw new UserFriendlyException("模板名称不能为空");
            await CheckSameName(input.Name, input.Id);
            if (input.Id == Guid.Empty || input.Id == null) throw new UserFriendlyException("当前数据错误，请刷新页面重新尝试");
            var daily = _dailyTemplateRepository.FirstOrDefault(x => x.Id == input.Id);
            if (input.IsDefault)
            {
                var query = _dailyTemplateRepository.FirstOrDefault(x => x.IsDefault &&x.Id !=input.Id);
                if (query != null)
                {
                    query.IsDefault = false;
                    await _dailyTemplateRepository.UpdateAsync(query);
                }
            }
            daily.Name = input.Name;
            daily.Description = input.Description;
            daily.IsDefault = input.IsDefault;
            daily.Remark = input.Remark;
            await _dailyTemplateRepository.UpdateAsync(daily);
            return true;
        }
        #region 私有方法
        private async Task<bool> CheckSameName(string name, Guid? id)
        {
            return await Task.Run(() =>
            {
                var sameNames = _dailyTemplateRepository.WhereIf(id != Guid.Empty && id != null, x => x.Id != id)
                .FirstOrDefault(a => a.Name == name);
                if (sameNames != null)
                {
                    throw new UserFriendlyException("当前模板名称已存在！");
                }

                return true;
            });
        }
        #endregion
    }
}
