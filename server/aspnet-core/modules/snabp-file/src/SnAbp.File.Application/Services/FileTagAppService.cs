/**********************************************************************
*******命名空间： SnAbp.File.Services
*******类 名 称： TagAppService
*******类 说 明： 资源标签维护接口实现
*******作    者： Easten
*******机器名称： EASTEN
*******CLR 版本： 4.0.30319.42000
*******创建时间： 2020/6/11 16:20:55
***********************************************************************
******* ★ Copyright @ 陕西心像科技 2020-2021. All rights reserved ★ *********
***********************************************************************
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SnAbp.File.Authorization;
using SnAbp.File.Dtos;
using SnAbp.File.Entities;
using SnAbp.File.IServices;
using SnAbp.Message;
using SnAbp.Message.Notice;
using SnAbp.Message.Service;

using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace SnAbp.File.Services
{
    [Authorize]
    public class FileTagAppService : FileAppService, IFileTagAppService
    {
        private readonly IGuidGenerator _generator;
        private readonly IFileManager _fileManager;
        private readonly IRepository<Tag, Guid> _tagResp;
        private readonly IMessageNoticeProvider _messageProvider;

        public FileTagAppService(
            IRepository<Tag, Guid> tagResp,
            IFileManager fileManager,
            IGuidGenerator generator,
            IMessageNoticeProvider messageProvider
        )
        {
            _tagResp = tagResp;
            _generator = generator;
            _fileManager = fileManager;
            _messageProvider = messageProvider;
        }

        /// <summary>
        ///     根据组织id获取该组织id下的标签列表
        /// </summary>
        /// <param name="id">指定的组织id</param>
        /// <returns>标签集合</returns>
        public Task<List<FileTagDto>> GetList(Guid id)
        {
            // 根据组织id 获取标签列表
            var list = _tagResp.WhereIf(id != null, a => a.OrganizationId == id).ToList();
            var data = ObjectMapper.Map<List<Tag>, List<FileTagDto>>(list);
            return Task.FromResult(data);
        }

        /// <summary>
        ///     新增一个资源标签
        /// </summary>
        /// <param name="input">输入的内容</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.Tag.Create)]
        public async Task<bool> Create(FileTagCreateDto input)
        {
            if (input.OrganizationId == null)
            {
                throw new UserFriendlyException("组织id不能为空");
            }
            var model = new Tag(_generator.Create());
            ObjectMapper.Map(input, model);
            await _tagResp.InsertAsync(model);
            return true;
        }

        /// <summary>
        ///     更新指定的标签
        /// </summary>
        /// <param name="input">需要更新的内容</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.Tag.Update)]
        public async Task<bool> Update(FileTagUpdateDto input)
        {
            var model = _tagResp.FirstOrDefault(a => a.Id == input.Id);
            if (model != null)
            {
                model.Name = input.Name;
                await _tagResp.UpdateAsync(model);
            }
            return true;
        }

        /// <summary>
        ///     删除指定的标签
        /// </summary>
        /// <param name="id">需要删除的标签的id</param>
        /// <returns></returns>
        //[Authorize(FilePermissions.Tag.Delete)]
        public async Task<bool> Delete(Guid id)
        {
            // 删除标签，由于标签有关联表，删除时需要友情提示
            try
            {
                await _tagResp.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("标签删除失败，此数据被其他数据数据关联");
            }
        }

        public async Task<List<Guid>> GetTagIds(ResourceType type, Guid id)
        {
            if (type == ResourceType.Folder)
            {
                var folder = await _fileManager.GetFolder(a => a.Id == id);
                return folder?.Tags?.Select(a => a.TagId)?.ToList();
            }
            if (type == ResourceType.File)
            {
                var file = await _fileManager.GetFile(a => a.Id == id);
                return file?.Tags?.Select(a => a.TagId)?.ToList();
            }
            return null;
        }
    }
}