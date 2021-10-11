using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.IServices
{
    public interface IProjectBooksClassificationAppService : IApplicationService
    {
        Task<bool> Create(BooksClassificationSimpleDto input); 
         Task<bool> Delete(Guid id);
        Task<bool> Update(BooksClassificationDto input);
        Task<BooksClassificationDto> Get(Guid id);
        Task<PagedResultDto<BooksClassificationDto>> GetList();
    }
}
