using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicQuotaItemAppService : IApplicationService
    {
        Task<PagedResultDto<QuotaItemDto>> GetList(QuotaItemGetListDto input);
        
        /// <summary>
        /// 获取清单
        /// </summary>
        /// <param name="id">定额Id</param>
        /// <param name="computerCodeId">电算代号Id</param>
        /// <returns></returns>
        Task<QuotaItemDto> Get(Guid id, Guid computerCodeId);
        Task<QuotaItemDto> Create(QuotaItemCreateDto input);
        Task<QuotaItemDto> Update(QuotaItemUpdateDto input);

        /// <summary>
        /// 删除清单
        /// </summary>
        /// <param name="id">定额Id</param>
        /// <param name="computerCodeId">电算代号Id</param>
        /// <returns></returns>
        Task<bool> Delete(Guid id,Guid computerCodeId);

        Task Upload([FromForm] ImportData input,Guid id);
        Task<Stream> Export(QuotaItemData input);
    }
}
