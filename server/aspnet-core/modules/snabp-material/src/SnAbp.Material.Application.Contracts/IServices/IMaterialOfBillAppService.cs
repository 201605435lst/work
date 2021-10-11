using SnAbp.Material.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Material.IServices
{
    public interface IMaterialOfBillAppService : IApplicationService
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MaterialOfBillDto> Get(Guid id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<MaterialOfBillDto>> GetList(MaterialOfBillSearchDto input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MaterialOfBillDto> Create(MaterialOfBillCreateDto input);


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MaterialOfBillDto> Update(MaterialOfBillUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Stream> Export(Guid id);
    }
}
