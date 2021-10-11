using Microsoft.AspNetCore.Authorization;
using SnAbp.Regulation.Dtos.Label;
using SnAbp.Regulation.Entities;
using SnAbp.Regulation.IServices.Label;
using SnAbp.Regulation.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.Regulation.Services
{
    [Authorize]
    public class RegulationLabelAppService : RegulationAppService, IRegulationLabelAppService
    {
        private readonly IRepository<Label, Guid> _repositoryLabel;
        private readonly IGuidGenerator _guidGenerate;
        public RegulationLabelAppService(IRepository<Label, Guid> repositoryLabel, IGuidGenerator guidGenerate)
        {
            _repositoryLabel = repositoryLabel;
            _guidGenerate = guidGenerate;
        }

        #region 创建标签
        [Authorize(RegulationPermissions.Label.Create)]
        public async Task<LabelDto> Create(LabelCreateDto input)
        {
            if (string.IsNullOrEmpty(input.Name)) throw new UserFriendlyException("标签名称不能为空");
            CheckSameName(input.Name);

            var label = new Label(_guidGenerate.Create());
            ObjectMapper.Map(input, label);
            await _repositoryLabel.InsertAsync(label);
            return ObjectMapper.Map<Label, LabelDto>(label);
        }
        #endregion

        #region 删除书签
        [Authorize(RegulationPermissions.Label.Delete)]
        public async Task<bool> Delete(List<Guid> ids)
        {
            await _repositoryLabel.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }
        #endregion

        #region 查询标签
        [Authorize(RegulationPermissions.Label.Detail)]
        public Task<LabelDto> Get(Guid id)
        {
            if (id == null || id == Guid.Empty) throw new UserFriendlyException("请输入正确的id");
            var label = _repositoryLabel.WithDetails().Where(x => x.Id == id).FirstOrDefault();
            if (label == null) throw new UserFriendlyException("此标签不存在");
            var result = ObjectMapper.Map<Label, LabelDto>(label);
            return Task.FromResult(result);
        }

        public Task<PagedResultDto<LabelDto>> GetList(LabelSearchDto input)
        {
           
            var labelList = _repositoryLabel.WithDetails()
                 .WhereIf(!string.IsNullOrEmpty(input.KeyWords), x => x.Name.Contains(input.KeyWords) || x.Classify.Contains(input.KeyWords));
            var result = new PagedResultDto<LabelDto>();
            result.TotalCount = labelList.Count();
            if (input.Order == "descend")
            {
                var list = labelList.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                result.Items = ObjectMapper.Map<List<Label>, List<LabelDto>>(list);
            }
            if (input.Order == "ascend")
            {
                var list = labelList.OrderBy(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
                result.Items = ObjectMapper.Map<List<Label>, List<LabelDto>>(list);
            }
            return Task.FromResult(result);
        }
        #endregion

        #region 修改书签
        [Authorize(RegulationPermissions.Label.Update)]
        public async Task<LabelDto> Update(LabelUpdateDto input)
        {
            var label = await _repositoryLabel.GetAsync(input.Id);
            if (label == null) throw new UserFriendlyException("当前标签不存在");
            label.Name = input.Name;
            label.Classify = input.Classify;
            await _repositoryLabel.UpdateAsync(label);
            return ObjectMapper.Map<Label, LabelDto>(label);
        }
        #endregion

        #region 判断标签是否已经存在
        private void CheckSameName(string name)
        {
            var result = _repositoryLabel.Where(x => x.Name == name);
            if (result.Count() > 0)
                throw new UserFriendlyException("标签名称已存在");
        }
        #endregion
    }
}
