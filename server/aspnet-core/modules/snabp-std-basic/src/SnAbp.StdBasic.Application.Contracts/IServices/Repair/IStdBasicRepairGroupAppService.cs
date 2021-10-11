using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicRepairGroupAppService : IApplicationService
    {
        /// <summary>
        /// 获取树状数据
        /// </summary>
        /// <returns></returns>
        PagedResultDto<RepairGroupSimpleDto> GetTreeList(RepairGroupGetListDto input);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<RepairGroupDto> Create(RepairGroupCreateDto input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(RepairGroupUpdateDto input);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
