/**********************************************************************
*******命名空间： SnAbp.Technology.Services
*******类 名 称： TechnologyDiscloseAppService
*******类 说 明： 技术交底服务
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2021/3/30 14:17:56
*******联系方式： 1301485237@qq.com
***********************************************************************
******* ★ Copyright @陕西心像 2020-2021. All rights reserved ★ *********
***********************************************************************
 */
using SnAbp.Technology.Dtos;
using SnAbp.Technology.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp;

namespace SnAbp.Technology.Services
{
    /// <summary>
    /// 技术交底服务
    /// </summary>
    public class TechnologyDiscloseAppService : TechnologyAppService
    {
        IRepository<Disclose> _discloseRepository;
        IGuidGenerator _guidGenerator;
        public TechnologyDiscloseAppService(
            IRepository<Disclose> discloseRepository,
            IGuidGenerator guidGenerator
            )
        {
            _discloseRepository = discloseRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<PagedResultDto<DiscloseDto>> GetList(DiscloseSearchDto input)
        {
            var result = new PagedResultDto<DiscloseDto>();
            var query = _discloseRepository.WithDetails(a => a.Parent)
                .WhereIf(!input.KeyWord.IsNullOrEmpty(), a => a.Name.Contains(input.KeyWord))
                .Where(a => a.Type == input.Type && a.ParentId == null);
            result.TotalCount = query.Count();
            result.Items = ObjectMapper.Map<List<Disclose>, List<DiscloseDto>>(query.OrderByDescending(y=>y.CreationTime).PageBy(input.SkipCount, input.MaxResultCount).ToList());
            return await Task.FromResult(result);
        }
        public async Task<List<DiscloseDto>> Get(Guid id)
        {
            var query = _discloseRepository.Where(a => a.ParentId == id);
            var list = ObjectMapper.Map<List<Disclose>, List<DiscloseDto>>(query.ToList());
            return await Task.FromResult(list);
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Create(DiscloseCreateDto input)
        {
            var model = ObjectMapper.Map<DiscloseCreateDto, Disclose>(input);
            model.SetId(_guidGenerator.Create());
            await _discloseRepository.InsertAsync(model);
            return true;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> Update(DiscloseCreateDto input)
        {
            var entity = await _discloseRepository.GetAsync(a => a.Id == input.Id);
            entity.Name = input.Name;
            entity.Url = input.Url;
            entity.Size = input.Size;
            await _discloseRepository.UpdateAsync(entity);
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new UserFriendlyException("请刷新页面重新尝试");
            }

            var entity = _discloseRepository.WithDetails().Where(x => x.ParentId == id).FirstOrDefault();
            if (entity != null)
            {
                throw new UserFriendlyException("请先删除他的附件数据");
            }
            await _discloseRepository.DeleteAsync(a => a.Id == id);
            return true;
        }

        public async Task<bool> DeleteRange(List<Guid> ids)
        {
            foreach (var id in ids)
            {
                if (id == null || id == Guid.Empty)
                {
                    throw new UserFriendlyException("数据错误，请刷新页面重新尝试");
                }

                var entity = _discloseRepository.WithDetails().Where(x => x.ParentId == id).FirstOrDefault();
                if (entity != null)
                {
                    var disclose = _discloseRepository.WithDetails().Where(x => x.Id == id).FirstOrDefault();
                    throw new UserFriendlyException($"{disclose.Name} 有附件信息,请先删除它的附件信息");
                }
            }
            await _discloseRepository.DeleteAsync(a => ids.Contains(a.Id));
            return true;
        }

        /// <summary>
        /// 更新附件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAttachment(DiscloseAttachmentCreateDto input)
        {
            if (input.Items.Any())
            {
                foreach (var item in input.Items)
                {
                    if (!_discloseRepository.Any(a => a.Id == item.Id))
                    {
                        var model = ObjectMapper.Map<DiscloseCreateDto, Disclose>(item);
                        model.SetId(_guidGenerator.Create());
                        model.ParentId = input.ParentId;
                        await _discloseRepository.InsertAsync(model);
                    }
                    else
                    {
                        if (item.IsDelete)
                        {
                            await _discloseRepository.DeleteAsync(a => a.Id == item.Id);
                        }
                    }
                }
            }
            return true;
        }
    }
}
