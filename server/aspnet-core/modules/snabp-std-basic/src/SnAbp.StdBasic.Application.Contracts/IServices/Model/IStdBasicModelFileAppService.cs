using SnAbp.StdBasic.Dtos.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.StdBasic.IServices
{
    public interface IStdBasicModelFileAppService : IApplicationService
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ModelFileDto> Create(ModelFileCreateDto input);

        Task<PagedResultDto<ModelFileDto>> GetList(Guid ModelId);

        Task<ModelFileDto> Update(ModelFileDto input);

        Task<bool> Delete(Guid id);
    }
}
