using SnAbp.CrPlan.Dto.MaintenanceRecord;
using SnAbp.CrPlan.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SnAbp.CrPlan.IServices.MaintenanceRecord
{
    public interface ICrPlanMaintenanceRecordAppService : IApplicationService
    {
        Task<PagedResultDto<MaintenanceRecordEquipDto>> GetList(MaintenanceRecordEquipSearchDto input);

        Task<PagedResultDto<MaintenanceRecordDto>> MaintenanceRecord(MaintenanceRecordSearchDto input);
    }
}
