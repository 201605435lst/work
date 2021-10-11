using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicComponentCategoryRltQuotaAppService : IApplicationService
    {

        /// <summary>
        /// 根据Id获取关联表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ComponentCategoryRltQuotaDto> Get(Guid id);

        /// <summary>
        /// 编辑关联关系列表（删除原表中相关数据，增加新数据）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ComponentCategoryRltQuotaDto>> EditList(ComponentCategoryRltQuotaCreateDto input);

        /// <summary>
        /// 根据产品Id获取关联的定额列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ComponentCategoryRltQuotaDto>> GetListByComponentCategoryId(RltSeachDto input);

        /// <summary>
        /// 删除关联表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
