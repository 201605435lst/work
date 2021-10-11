using SnAbp.Bpm.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SnAbp.Message.Bpm.Entities;
using Volo.Abp.Application.Dtos;
using SnAbp.Message.Bpm.Dtos;
using Volo.Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using SnAbp.Message.Bpm.Repositorys;
using SnAbp.Message.Bpm.Services;
using Volo.Abp;

namespace SnAbp.Message.Bpm
{
    public abstract class BpmMessageBaseController : AbpController
    {
        protected BpmMessageBaseController()
        {
            LocalizationResource = typeof(BpmResource);
        }
    }

    [Route("api/message/bpmMessage")]
    public class BpmMessageController : BpmMessageBaseController
    {
        private readonly IBpmMessageRepository _bpmMessageRepository;

        public BpmMessageController(IBpmMessageRepository bpmMessageRepository)
        {
            _bpmMessageRepository = bpmMessageRepository;
        }

        [HttpGet, Route("getList")]
        public async Task<PagedResultDto<BpmRltMessage>> GetList(BpmMessageSearchDto input)
        {
            var bpmMessage = await _bpmMessageRepository.GetList(input.Keyword, input.IsProcess);
            var result = new PagedResultDto<BpmRltMessage>();
            result.TotalCount = bpmMessage.Count();
            result.Items = ObjectMapper.Map<List<BpmRltMessage>, List<BpmRltMessage>>(bpmMessage.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount).ToList());
            return await Task.FromResult(result);
        }

        [HttpPut, Route("updateRange")]
        public Task<BpmRltMessage> UpdateRange(List<Guid> ids)
        {
           return _bpmMessageRepository.UpdateRange(ids);
        }

        [HttpDelete, Route("delete")]
        public Task<bool> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new UserFriendlyException("该消息不存在");
            return _bpmMessageRepository.Delete(id);
        }
    }
}
