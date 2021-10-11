using SnAbp.Resource.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SnAbp.Resource.IServices
{
    public interface IResourceTerminalBusinessAppService : IApplicationService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TerminalBusinessPathDto> Get(Guid id);


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TerminalBusinessPathDto> create(TerminalBusinessPathDto input);


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        Task<TerminalBusinessPathDto> update(TerminalBusinessPathDto input);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> delete(Guid id);
    }
}
