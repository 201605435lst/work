using SnAbp.Project.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.Project.IServices.Dossier
{
    public interface IProjectDossierCategoryAppService : IApplicationService
    {

        Task<List<FileClassificationDto>> GetList(FileClassificationSearchDto input);

    }
}
