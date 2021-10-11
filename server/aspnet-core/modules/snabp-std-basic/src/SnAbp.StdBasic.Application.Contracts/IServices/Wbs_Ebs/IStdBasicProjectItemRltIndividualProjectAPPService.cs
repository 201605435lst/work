using SnAbp.StdBasic.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicProjectItemRltIndividualProjectAPPService : IApplicationService
    {

        /// <summary>
        /// 根据Id获取关联表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProjectItemRltIndividualProjectDto> Get(Guid id);

        /// <summary>
        /// 编辑关联关系列表（删除原表中相关数据，增加新数据）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectItemRltIndividualProjectDto>> EditList(ProjectItemRltIndividualProjectCreateDto input);

        /// <summary>
        /// 根据单项工程Id获取关联的工程工项列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<ProjectItemRltIndividualProjectDto>> GetListByIndividualProjectId(RltSeachDto input);

        /// <summary>
        /// 删除关联表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);


    }
}
