using SnAbp.Emerg.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Emerg
{
    public interface IOrganizationAppService : IApplicationService
    {
        Task<List<OrganizationDto>> GetListAsync(OrganizationGetDto input);

        Task<OrganizationDto> CreateAsync(OrganizationInputDto input);

        Task<bool> UpdateAsync(OrganizationUpdateDto input);

        Task<bool> DeleteAsync(Guid id);

        Task<string> Test(string txt);

        Task<OrganizationDto> GetAsync(Guid id);
    }
}
