using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SnAbp.StdBasic.Dtos.Repair.RepairItem;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicRepairItemAppService : IApplicationService
    {
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<RepairItemDetailDto> Get(Guid id);

        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<RepairItemDto>> GetList(RepairItemSearchDto input);

        /// <summary>
        /// 获取最大编号
        /// </summary>
        /// <returns></returns>
        Task<int> GetMaxNumber(GetMaxNumberDto input);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RepairItemDto> Create(RepairItemCreateDto input);
        /// <summary>
        /// 标签迁移
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CreateTagMigration(RepairItemTagMigratioDto input);
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(RepairItemUpdateDto input);

        Task<bool> UpdateOrganizationType(RepairItemUpdateSimpleDto input);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
