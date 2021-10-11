using SnAbp.StdBasic.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicRepairTestItemAppService : IApplicationService
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RepairTestItemDto> get(Guid id);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RepairTestItemDto> Create(RepairTestItemCreateDto input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(RepairTestItemUpdateDto input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> UpdateOrder(RepairTestItemSimpleDto input);
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
