using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SnAbp.StdBasic.Repositories
{
    public interface IRepairItemRepository : IRepository<Entities.RepairItem, Guid>
    {
        Task<bool> InsertRange(IEnumerable<Entities.RepairItem> list);
    }
}
