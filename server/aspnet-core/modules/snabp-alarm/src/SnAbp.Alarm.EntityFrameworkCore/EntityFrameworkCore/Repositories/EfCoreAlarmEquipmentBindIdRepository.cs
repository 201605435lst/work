using SnAbp.Alarm.IRepositories;
using System;
using SnAbp.EntityFrameworkCore;
using SnAbp.Domain.Repositories.EntityFrameworkCore;

namespace SnAbp.Alarm.EntityFrameworkCore.Repositories
{
    public class EfCoreAlarmEquipmentBindIdRepository :
        EfCoreRepository<IAlarmDbContext, Entities.AlarmEquipmentIdBind, Guid>,
        IEfCoreAlarmEquipmentBindIdRepository
    {
        public EfCoreAlarmEquipmentBindIdRepository(IDbContextProvider<IAlarmDbContext> dbContextProvider) : base(dbContextProvider) { }
    }
}
