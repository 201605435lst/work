using SnAbp.Oa.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Oa.IServices
{
    public interface IOaContractAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ContractDto> Get(Guid id);
        /// <summary>
        /// 获取编号最大的合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ContractDto> GetMaxCode();
        /// <summary>
        /// <summary>
        /// 获取合同列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ContractDto>> GetList(ContractSearchDto input);
        /// <summary>
        /// 创建合同
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ContractDto> Create(ContractCreateDto input);
        /// <summary>
        /// 创建合同
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ContractDto> Update(ContractUpdateDto input);
        /// <summary>
        /// 导出Ecel文件
        /// </summary>
        Task<Stream> DownLoad(List<Guid> ids);
        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> ids);

    }
}
