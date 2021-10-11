using Microsoft.AspNetCore.Mvc;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Export;
using SnAbp.StdBasic.Dtos.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
   public interface IStdBasicIndividualProjectAppService : IApplicationService
    {
        /// <summary>
        /// 获取单个单项工程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IndividualProjectDto> Get(Guid id);

        /// <summary>
        /// 根据输入条件获取单项工程树
        /// </summary>
        /// <param name="input">输入条件</param>
        /// <returns></returns>
        Task<PagedResultDto<IndividualProjectDto>> GetList(IndividualProjectGetListByIdsDto input);
        /// <summary>
        /// 根据Ids获取当前对象
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<IndividualProjectDto>> GetListIndividualProject(IndividualProjectGetListByIdsDto input);
        /// <summary>
        /// 根据id获得当前单项工程中的最大编码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IndividualProjectDto> GetListCode(Guid? id);


        /// <summary>
        /// 创建单项工程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IndividualProjectDto> Create(IndividualProjectCreateDto input);
        /// <summary>
        /// 修改单项工程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IndividualProjectDto> Update(IndividualProjectUpdateDto input);
        /// <summary>
        /// 删除单项工程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(Guid id);

        Task Upload([FromForm] ImportData input);
        Task<Stream> Export(IndividualProjectData input);

    }
}
