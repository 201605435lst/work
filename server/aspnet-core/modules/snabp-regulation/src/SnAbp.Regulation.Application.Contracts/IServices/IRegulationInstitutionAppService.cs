using SnAbp.Regulation.Dtos.Institution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Regulation.IServices
{
    public interface IRegulationInstitutionAppService:IApplicationService
    {

        /// <summary>
        /// 创建制度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InstitutionDto> Create(InstitutionCreateDto input);

        /// <summary>
        /// 删除制度
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Delete(List<Guid> ids);

        /// <summary>
        /// 修改制度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InstitutionDto> Update(InstitutionUpdateDto input);

        /// <summary>
        /// 查询制度
        /// </summary>
        Task<PagedResultDto<InstitutionDto>> GetList(InstitutionSearchDto input);
        Task<InstitutionDto> Get(Guid id);

        /// <summary>
        /// 导出Ecel文件
        /// </summary>
        Task<Stream> DownLoad(List<Guid> ids);
    }
}
