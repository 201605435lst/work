using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.StdBasic.Repositories
{
    public class RepairItemRepository : EfCoreRepository<StdBasicDbContext, RepairItem, Guid>, IRepairItemRepository
    {
        private IDbContextProvider<StdBasicDbContext> _dbContextProvider;

        public RepairItemRepository(IDbContextProvider<StdBasicDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public Task<bool> InsertRange(IEnumerable<RepairItem> list)
        {
            DbContext.Set<RepairItem>().AddRange(list);
            return Task.FromResult(DbContext.SaveChanges() == list.Count());
        }
    }
}
