using SnAbp.Oa.Dtos.Seal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Oa.IServices
{
    public interface IOaSealAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个签章信息表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SealDto> Get(Guid id);
        /// <summary>
        /// 获取签章信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<SealSimpleDto>> GetList(SealSearchDto input);
        /// <summary>
        /// 创建签章信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SealDto> Create(SealCreateDto input);

        /// <summary>
        /// 加密Or解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SealDto> Update(SealUpdateDto input);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> ids);
    }
}
