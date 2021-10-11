using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicStandardEquipmentTerminalAppService : IApplicationService
    {

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<StandardEquipmentTerminalDto> Get(Guid id);

        /// <summary>
        /// 根据条件获取分页数据
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<StandardEquipmentTerminalDto>> GetList(StandardEquipmentTerminalSearchInputDto input);

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StandardEquipmentTerminalDto> Create(StandardEquipmentTerminalCreateDto input);

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<StandardEquipmentTerminalDto> Update(StandardEquipmentTerminalUpdateDto input);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

    }
}
