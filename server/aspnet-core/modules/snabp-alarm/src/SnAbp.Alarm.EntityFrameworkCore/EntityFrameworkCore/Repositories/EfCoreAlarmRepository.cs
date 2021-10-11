using SnAbp.Alarm.IRepositories;
using System;
using SnAbp.Domain.Repositories.EntityFrameworkCore;
using SnAbp.EntityFrameworkCore;

namespace SnAbp.Alarm.EntityFrameworkCore.Repositories
{
    public class EfCoreAlarmRepository :
        EfCoreRepository<IAlarmDbContext, Entities.Alarm, Guid>,
        IEfCoreAlarmRepository
    {
        public EfCoreAlarmRepository(IDbContextProvider<IAlarmDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}
