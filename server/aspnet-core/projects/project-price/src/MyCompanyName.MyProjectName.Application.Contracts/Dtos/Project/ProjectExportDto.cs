using MyCompanyName.MyProjectName.Dtos.Project;
using SnAbp.Utils.DataImport;
using SnAbp.Utils.TreeHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities.Auditing;

namespace MyCompanyName.MyProjectName.Dtos
{
    /// <summary>
    /// 项目
    /// </summary>
    public class ProjectExportDto : FileExportDto
    {
        public ProjectExportDtoParamter paramter;
    }
}
