using MyCompanyName.MyProjectName.Dtos;
using SnAbp.Utils.DataImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName.IServices
{
    public interface IProjectPriceProjectAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="cableId"></param>
        /// <returns></returns>
        Task<List<ProjectDto>> GetList(ProjectGetListInput input);

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectDto> Get(Guid id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Create(ProjectCreateDto input);

        public Task<Stream> Export(ProjectExportDto input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> Update(ProjectUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);
    }
}
