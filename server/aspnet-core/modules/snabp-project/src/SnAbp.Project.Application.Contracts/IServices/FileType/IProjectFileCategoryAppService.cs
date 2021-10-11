using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.IServices
{
    public interface IProjectFileCategoryAppService:IApplicationService
    {

        Task<bool> Create(FileCategoryDto input);
        Task<bool> Delete(Guid id);
        Task<bool> Update(FileCategoryDto input);
        Task<FileCategoryDto> Get(Guid id);
        Task<PagedResultDto<FileCategoryDto>> GetList();

       
    }
}
