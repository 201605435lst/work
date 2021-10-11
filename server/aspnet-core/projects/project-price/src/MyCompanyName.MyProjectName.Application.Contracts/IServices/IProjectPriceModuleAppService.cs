using MyCompanyName.MyProjectName.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName.IServices
{
    public interface IProjectPriceModuleAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="cableId"></param>
        /// <returns></returns>
        Task<List<ModuleDto>> GetList(ModuleGetListInput input);

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ModuleDto> Get(Guid id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Create(ModuleCreateDto input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(ModuleUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
